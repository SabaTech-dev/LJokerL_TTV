using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using LJokerL.Core.Models;

namespace LJokerL.Core.Services;

public sealed class SystemMetricsService
{
    private readonly HardwareDetector _hardwareDetector = new();
    private readonly CpuUsageSampler _cpuSampler = new();
    private readonly NetworkUsageSampler _networkSampler = new();
    private readonly GpuUsageSampler _gpuSampler = new();
    private string? _cpuName;
    private string? _gpuName;
    private ulong? _gpuVram;
    private DateTime _lastTempSampleUtc;
    private double? _lastTemperatureC;

    public SystemMetricsSnapshot CaptureSnapshot()
    {
        var nowUtc = DateTime.UtcNow;
        EnsureHardwareCache();

        var cpuUsage = _cpuSampler.GetUsagePercent();
        var memory = GetMemoryMetrics();
        var disks = GetDiskMetrics();
        var network = _networkSampler.GetNetworkMetrics();
        var processes = GetTopProcesses();
        var temperature = GetTemperatureCelsius(nowUtc);
        var gpuUsage = _gpuSampler.GetUsagePercent();

        return new SystemMetricsSnapshot
        {
            TimestampUtc = nowUtc,
            Cpu = new CpuMetrics
            {
                Name = _cpuName,
                UsagePercent = cpuUsage,
                TemperatureCelsius = temperature
            },
            Gpu = new GpuMetrics
            {
                Name = _gpuName,
                UsagePercent = gpuUsage,
                VramBytes = _gpuVram
            },
            Memory = memory,
            Disks = disks,
            Network = network,
            Processes = processes
        };
    }

    private void EnsureHardwareCache()
    {
        if (!string.IsNullOrWhiteSpace(_cpuName) || !string.IsNullOrWhiteSpace(_gpuName))
        {
            return;
        }

        try
        {
            var snapshot = _hardwareDetector.Detect();
            _cpuName = snapshot.Cpu?.Name;
            _gpuName = snapshot.Gpu?.Name;
            _gpuVram = snapshot.Gpu?.VramBytes;
        }
        catch
        {
        }
    }

    private static MemoryMetrics GetMemoryMetrics()
    {
        var status = new MemoryStatusEx();
        if (!GlobalMemoryStatusEx(ref status))
        {
            return new MemoryMetrics();
        }

        var total = status.ullTotalPhys;
        var available = status.ullAvailPhys;
        var used = total > available ? total - available : 0;
        var usage = total == 0 ? 0 : used * 100.0 / total;

        return new MemoryMetrics
        {
            TotalBytes = total,
            AvailableBytes = available,
            UsagePercent = Math.Round(usage, 2)
        };
    }

    private static List<DiskMetrics> GetDiskMetrics()
    {
        var disks = new List<DiskMetrics>();
        try
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType != DriveType.Fixed || !drive.IsReady)
                {
                    continue;
                }

                var total = (ulong)drive.TotalSize;
                var free = (ulong)drive.AvailableFreeSpace;
                var used = total > free ? total - free : 0;
                var usage = total == 0 ? 0 : used * 100.0 / total;

                disks.Add(new DiskMetrics
                {
                    Name = drive.Name.TrimEnd('\\'),
                    TotalBytes = total,
                    FreeBytes = free,
                    UsagePercent = Math.Round(usage, 2)
                });
            }
        }
        catch
        {
        }

        return disks;
    }

    private static List<ProcessMetrics> GetTopProcesses()
    {
        var results = new List<ProcessMetrics>();
        try
        {
            var processes = Process.GetProcesses()
                .OrderByDescending(process =>
                {
                    try
                    {
                        return process.WorkingSet64;
                    }
                    catch
                    {
                        return 0;
                    }
                })
                .Take(8);

            foreach (var process in processes)
            {
                try
                {
                    results.Add(new ProcessMetrics
                    {
                        Name = process.ProcessName,
                        Id = process.Id,
                        MemoryMb = Math.Round(process.WorkingSet64 / 1024d / 1024d, 2),
                        CpuMinutes = Math.Round(process.TotalProcessorTime.TotalMinutes, 2)
                    });
                }
                catch
                {
                }
            }
        }
        catch
        {
        }

        return results;
    }

    private double? GetTemperatureCelsius(DateTime nowUtc)
    {
        if (_lastTempSampleUtc != DateTime.MinValue && nowUtc - _lastTempSampleUtc < TimeSpan.FromSeconds(10))
        {
            return _lastTemperatureC;
        }

        _lastTempSampleUtc = nowUtc;
        var temperature = TryReadTemperatureWmi();
        if (temperature.HasValue)
        {
            _lastTemperatureC = temperature;
        }

        return _lastTemperatureC;
    }

    private static double? TryReadTemperatureWmi()
    {
        // Try to use System.Management via reflection if available
        // This allows the app to compile without the package, but use it if present at runtime
        try
        {
            var managementObjectSearcherType = Type.GetType("System.Management.ManagementObjectSearcher, System.Management");
            if (managementObjectSearcherType == null)
                return null;

            var instance = Activator.CreateInstance(
                managementObjectSearcherType,
                @"root\wmi",
                "SELECT CurrentTemperature FROM MSAcpi_ThermalZoneTemperature");
            if (instance is not IDisposable searcher)
                return null;

            var getMethod = managementObjectSearcherType.GetMethod("Get");
            var results = getMethod?.Invoke(searcher, null);
            if (results == null) return null;

            double? maxTemp = null;
            foreach (var obj in (System.Collections.IEnumerable)results)
            {
                using (obj as IDisposable)
                {
                    var itemType = obj.GetType();
                    var tempProp = itemType.GetProperty("Item", new[] { typeof(string) });
                    var tempValue = tempProp?.GetValue(obj, new object[] { "CurrentTemperature" });
                    
                    if (tempValue != null)
                    {
                        int tempKelvin = Convert.ToInt32(tempValue);
                        double celsius = (tempKelvin / 10.0) - 273.15;
                        
                        if (celsius >= -10 && celsius <= 120)
                        {
                            if (maxTemp == null || celsius > maxTemp)
                                maxTemp = celsius;
                        }
                    }
                }
            }
            
            return maxTemp.HasValue ? Math.Round(maxTemp.Value, 1) : null;
        }
        catch
        {
            return null;
        }
    }

    private sealed class CpuUsageSampler
    {
        private ulong _lastIdle;
        private ulong _lastTotal;
        private bool _hasSample;

        public double GetUsagePercent()
        {
            if (!GetSystemTimes(out var idle, out var kernel, out var user))
            {
                return 0;
            }

            var idleTime = idle.ToUInt64();
            var totalTime = kernel.ToUInt64() + user.ToUInt64();

            if (!_hasSample)
            {
                _lastIdle = idleTime;
                _lastTotal = totalTime;
                _hasSample = true;
                return 0;
            }

            var idleDelta = idleTime - _lastIdle;
            var totalDelta = totalTime - _lastTotal;
            _lastIdle = idleTime;
            _lastTotal = totalTime;

            if (totalDelta == 0)
            {
                return 0;
            }

            var usage = (totalDelta - idleDelta) * 100.0 / totalDelta;
            return Math.Round(Math.Clamp(usage, 0, 100), 2);
        }
    }

    private sealed class NetworkUsageSampler
    {
        private long _lastReceived;
        private long _lastSent;
        private DateTime _lastSampleUtc;
        private bool _hasSample;

        public NetworkMetrics GetNetworkMetrics()
        {
            var nowUtc = DateTime.UtcNow;
            var adapterCount = 0;
            long totalReceived = 0;
            long totalSent = 0;

            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus != OperationalStatus.Up)
                {
                    continue;
                }

                if (nic.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                    nic.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                {
                    continue;
                }

                adapterCount++;
                try
                {
                    var stats = nic.GetIPv4Statistics();
                    totalReceived += stats.BytesReceived;
                    totalSent += stats.BytesSent;
                }
                catch
                {
                }
            }

            double rxRate = 0;
            double txRate = 0;
            if (_hasSample)
            {
                var elapsed = (nowUtc - _lastSampleUtc).TotalSeconds;
                if (elapsed > 0)
                {
                    rxRate = (totalReceived - _lastReceived) / elapsed;
                    txRate = (totalSent - _lastSent) / elapsed;
                }
            }

            _lastSampleUtc = nowUtc;
            _lastReceived = totalReceived;
            _lastSent = totalSent;
            _hasSample = true;

            return new NetworkMetrics
            {
                AdapterCount = adapterCount,
                BytesReceivedPerSec = Math.Max(0, rxRate),
                BytesSentPerSec = Math.Max(0, txRate)
            };
        }
    }

    private sealed class GpuUsageSampler
    {
        private PerformanceCounter[]? _counters;
        private bool _initialized;
        private DateTime _lastInitAttempt;

        public double? GetUsagePercent()
        {
            if (!_initialized)
            {
                Initialize();
            }

            if (_counters == null || _counters.Length == 0)
            {
                return null;
            }

            double total = 0;
            int valid = 0;
            foreach (var counter in _counters)
            {
                try
                {
                    var val = counter.NextValue();
                    if (val >= 0 && val <= 100)
                    {
                        total += val;
                        valid++;
                    }
                }
                catch
                {
                }
            }

            return valid > 0 ? Math.Round(total / valid, 2) : null;
        }

        private void Initialize()
        {
            if (DateTime.Now - _lastInitAttempt < TimeSpan.FromMinutes(1))
            {
                _initialized = true;
                return;
            }

            _lastInitAttempt = DateTime.Now;
            _initialized = true;

            try
            {
                var category = new PerformanceCounterCategory("GPU Engine");
                var instanceNames = category.GetInstanceNames()
                    .Where(name => name.Contains("engtype_3D", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                if (instanceNames.Length == 0)
                {
                    _counters = null;
                    return;
                }

                _counters = instanceNames
                    .Select(name => new PerformanceCounter("GPU Engine", "Utilization Percentage", name))
                    .ToArray();

                foreach (var counter in _counters)
                {
                    try
                    {
                        _ = counter.NextValue();
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
                _counters = null;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct MemoryStatusEx
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;

        public MemoryStatusEx()
        {
            dwLength = (uint)Marshal.SizeOf(typeof(MemoryStatusEx));
            dwMemoryLoad = 0;
            ullTotalPhys = 0;
            ullAvailPhys = 0;
            ullTotalPageFile = 0;
            ullAvailPageFile = 0;
            ullTotalVirtual = 0;
            ullAvailVirtual = 0;
            ullAvailExtendedVirtual = 0;
        }
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool GlobalMemoryStatusEx(ref MemoryStatusEx lpBuffer);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetSystemTimes(out FileTime idleTime, out FileTime kernelTime, out FileTime userTime);

    [StructLayout(LayoutKind.Sequential)]
    private struct FileTime
    {
        public uint dwLowDateTime;
        public uint dwHighDateTime;

        public ulong ToUInt64()
        {
            return ((ulong)dwHighDateTime << 32) | dwLowDateTime;
        }
    }
}

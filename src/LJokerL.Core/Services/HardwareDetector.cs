using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using LJokerL.Core.Models;
using Microsoft.Win32;

namespace LJokerL.Core.Services;

public sealed class HardwareDetector
{
    public HardwareSnapshot Detect(bool? vrrOverride = null)
    {
        return new HardwareSnapshot
        {
            DetectedAtUtc = DateTime.UtcNow,
            Cpu = TryGetCpuInfo(),
            Gpu = TryGetGpuInfo(),
            Memory = TryGetMemoryInfo(),
            Os = TryGetOsInfo(),
            Monitor = TryGetMonitorInfo(vrrOverride)
        };
    }

    private static CpuInfo? TryGetCpuInfo()
    {
        try
        {
            using var cpuKey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
            if (cpuKey == null)
            {
                return null;
            }

            var name = cpuKey.GetValue("ProcessorNameString")?.ToString();
            var mhz = cpuKey.GetValue("~MHz");
            var maxClock = mhz != null ? Convert.ToInt32(mhz) : (int?)null;

            using var biosKey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\BIOS");
            var manufacturer = biosKey?.GetValue("SystemManufacturer")?.ToString();

            return new CpuInfo
            {
                Name = name,
                Manufacturer = manufacturer,
                Cores = null,
                LogicalProcessors = Environment.ProcessorCount,
                MaxClockMhz = maxClock
            };
        }
        catch
        {
            return null;
        }
    }

    private static MemoryInfo? TryGetMemoryInfo()
    {
        try
        {
            var status = new MemoryStatusEx();
            if (!GlobalMemoryStatusEx(ref status))
            {
                return null;
            }

            return new MemoryInfo
            {
                TotalBytes = status.ullTotalPhys,
                AverageSpeedMhz = null
            };
        }
        catch
        {
            return null;
        }
    }

    private static GpuInfo? TryGetGpuInfo()
    {
        var json = RunPowerShell("Get-CimInstance Win32_VideoController | Select-Object Name,AdapterCompatibility,AdapterRAM | ConvertTo-Json -Depth 2");
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind == JsonValueKind.Array)
            {
                GpuInfo? best = null;
                foreach (var element in doc.RootElement.EnumerateArray())
                {
                    var candidate = ParseGpuElement(element);
                    if (candidate == null)
                    {
                        continue;
                    }

                    if (best == null || (candidate.VramBytes ?? 0) > (best.VramBytes ?? 0))
                    {
                        best = candidate;
                    }
                }

                return best;
            }

            if (doc.RootElement.ValueKind == JsonValueKind.Object)
            {
                return ParseGpuElement(doc.RootElement);
            }
        }
        catch
        {
        }

        return null;
    }

    private static OsInfo? TryGetOsInfo()
    {
        try
        {
            var version = Environment.OSVersion.Version;
            return new OsInfo
            {
                Caption = RuntimeInformation.OSDescription,
                Version = version.ToString(),
                BuildNumber = version.Build.ToString()
            };
        }
        catch
        {
            return null;
        }
    }

    private static MonitorInfo? TryGetMonitorInfo(bool? vrrOverride)
    {
        return new MonitorInfo
        {
            Name = null,
            VrrSupported = vrrOverride ?? ReadVrrOverride()
        };
    }

    private static string RunPowerShell(string command)
    {
        var shell = GetPowerShellCommand();
        var psi = new ProcessStartInfo(shell, $"-NoProfile -ExecutionPolicy Bypass -Command \"{command}\"")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            using var process = Process.Start(psi);
            if (process == null)
            {
                return string.Empty;
            }

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                return string.Empty;
            }

            return string.IsNullOrWhiteSpace(output) ? error : output;
        }
        catch
        {
            return string.Empty;
        }
    }

    private static string GetPowerShellCommand()
    {
        var overridePath = Environment.GetEnvironmentVariable("LJOKERL_POWERSHELL_PATH");
        if (!string.IsNullOrWhiteSpace(overridePath))
        {
            return overridePath;
        }

        return "powershell";
    }

    private static GpuInfo? ParseGpuElement(JsonElement element)
    {
        var name = TryGetString(element, "Name");
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var vendor = TryGetString(element, "AdapterCompatibility");
        var vram = TryGetULong(element, "AdapterRAM");
        return new GpuInfo
        {
            Name = name,
            Vendor = vendor,
            VramBytes = vram
        };
    }

    private static string? TryGetString(JsonElement element, string property)
    {
        if (!element.TryGetProperty(property, out var value))
        {
            return null;
        }

        return value.ValueKind == JsonValueKind.String ? value.GetString() : value.ToString();
    }

    private static ulong? TryGetULong(JsonElement element, string property)
    {
        if (!element.TryGetProperty(property, out var value))
        {
            return null;
        }

        if (value.ValueKind == JsonValueKind.Number && value.TryGetUInt64(out var numeric))
        {
            return numeric;
        }

        if (value.ValueKind == JsonValueKind.String && ulong.TryParse(value.GetString(), out var parsed))
        {
            return parsed;
        }

        return null;
    }

    private static bool? ReadVrrOverride()
    {
        var raw = Environment.GetEnvironmentVariable("LJOKERL_VRR_OVERRIDE");
        if (string.IsNullOrWhiteSpace(raw))
        {
            return null;
        }

        var value = raw.Trim().ToLowerInvariant();
        if (value is "1" or "true" or "yes" or "on")
        {
            return true;
        }

        if (value is "0" or "false" or "no" or "off")
        {
            return false;
        }

        return null;
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
}

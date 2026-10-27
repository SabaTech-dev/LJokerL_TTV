using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using LJokerL.Core.Models;
using Microsoft.Win32;

namespace LJokerL.Core.Services;

public sealed class SystemProfiler
{
    private readonly GpuDetector _gpuDetector = new();
    private readonly StorageDetector _storageDetector = new();
    private readonly MonitorDetector _monitorDetector = new();
    private readonly SoftwareDetector _softwareDetector = new();

    public ExtendedHardwareSnapshot Profile()
    {
        var snapshot = new ExtendedHardwareSnapshot
        {
            DetectedAtUtc = DateTime.UtcNow,
            Cpu = ProfileCpu(),
            Gpus = ProfileGpus(),
            Memory = ProfileMemory(),
            Storage = ProfileStorage(),
            Monitors = ProfileMonitors(),
            Motherboard = ProfileMotherboard(),
            NetworkAdapters = ProfileNetwork(),
            Power = ProfilePower(),
            Software = _softwareDetector.Detect()
        };

        return snapshot;
    }

    public ExtendedHardwareSnapshot ProfileQuick()
    {
        var snapshot = new ExtendedHardwareSnapshot
        {
            DetectedAtUtc = DateTime.UtcNow,
            Cpu = ProfileCpuQuick(),
            Gpus = ProfileGpusQuick(),
            Memory = ProfileMemoryQuick(),
            Storage = ProfileStorageQuick(),
            Monitors = ProfileMonitorsQuick(),
            Motherboard = new MotherboardInfo(),
            NetworkAdapters = new List<NetworkAdapterInfo>(),
            Power = ProfilePowerQuick(),
            Software = new SoftwareEnvironmentInfo()
        };

        return snapshot;
    }

    private CpuInfoExtended ProfileCpu()
    {
        var cpu = new CpuInfoExtended();

        try
        {
            using var cpuKey = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
            if (cpuKey != null)
            {
                cpu.Name = cpuKey.GetValue("ProcessorNameString")?.ToString();
                cpu.Manufacturer = cpuKey.GetValue("VendorIdentifier")?.ToString();
                
                var mhz = cpuKey.GetValue("~MHz");
                cpu.MaxClockMhz = mhz != null ? Convert.ToInt32(mhz) : (int?)null;
                cpu.BaseClockMhz = cpu.MaxClockMhz;

                var family = cpuKey.GetValue("Family");
                var model = cpuKey.GetValue("Model");
                var stepping = cpuKey.GetValue("Stepping");
                
                if (cpu.Name != null)
                {
                    cpu.Vendor = DetectCpuVendor(cpu.Name);
                    cpu.Architecture = DetectCpuArchitecture(cpu.Name, cpu.Vendor);
                    cpu.Generation = DetectCpuGeneration(cpu.Name, cpu.Vendor);
                    cpu.IsHybrid = IsHybridCpu(cpu.Name);
                }
            }

            cpu.LogicalProcessors = Environment.ProcessorCount;
            cpu.PhysicalCores = GetPhysicalCoreCount();

            if (cpu.Name != null && cpu.IsHybrid)
            {
                cpu.PerformanceCores = cpu.PhysicalCores.HasValue ? cpu.PhysicalCores / 2 : null;
                cpu.EfficiencyCores = cpu.PhysicalCores.HasValue ? cpu.PhysicalCores / 2 : null;
            }

            using var biosKey = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            cpu.BiosVersion = biosKey?.GetValue("SystemBiosVersion")?.ToString();

            cpu.TdpWatts = EstimateCpuTdp(cpu.Name);
            cpu.IsMobile = IsMobileCpu(cpu.Name);
            cpu.Socket = DetectCpuSocket(cpu.Name);

            cpu.L1CacheKb = 32 * (cpu.PhysicalCores ?? 4);
            cpu.L2CacheKb = 512 * (cpu.PhysicalCores ?? 4);
            cpu.L3CacheKb = 8192 * ((cpu.PhysicalCores ?? 4) / 4);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SystemProfiler CPU error: {ex.Message}");
        }

        return cpu;
    }

    private CpuInfoExtended ProfileCpuQuick()
    {
        var cpu = new CpuInfoExtended();

        try
        {
            using var cpuKey = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
            if (cpuKey != null)
            {
                cpu.Name = cpuKey.GetValue("ProcessorNameString")?.ToString();
                var mhz = cpuKey.GetValue("~MHz");
                cpu.MaxClockMhz = mhz != null ? Convert.ToInt32(mhz) : (int?)null;
            }

            cpu.LogicalProcessors = Environment.ProcessorCount;
            cpu.PhysicalCores = GetPhysicalCoreCount();

            if (cpu.Name != null)
            {
                cpu.Vendor = DetectCpuVendor(cpu.Name);
                cpu.IsMobile = IsMobileCpu(cpu.Name);
            }
        }
        catch
        {
        }

        return cpu;
    }

    private List<GpuInfoExtended> ProfileGpus()
    {
        return _gpuDetector.DetectAll();
    }

    private List<GpuInfoExtended> ProfileGpusQuick()
    {
        try
        {
            return _gpuDetector.DetectAll();
        }
        catch
        {
            return new List<GpuInfoExtended>();
        }
    }

    private MemoryInfoExtended ProfileMemory()
    {
        var memory = new MemoryInfoExtended();

        try
        {
            var status = new MemoryStatusEx();
            if (GlobalMemoryStatusEx(ref status))
            {
                memory.TotalBytes = status.ullTotalPhys;
            }

            memory.SpeedMhz = GetMemorySpeed();
            memory.MemoryType = DetectMemoryType();
            memory.SlotsTotal = GetMemorySlotsTotal();
            memory.SlotsUsed = GetMemorySlotsUsed();
            memory.IsDualChannel = memory.SlotsUsed >= 2;
            
            memory.Modules = GetMemoryModules();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SystemProfiler Memory error: {ex.Message}");
        }

        return memory;
    }

    private MemoryInfoExtended ProfileMemoryQuick()
    {
        var memory = new MemoryInfoExtended();

        try
        {
            var status = new MemoryStatusEx();
            if (GlobalMemoryStatusEx(ref status))
            {
                memory.TotalBytes = status.ullTotalPhys;
            }

            memory.SpeedMhz = GetMemorySpeed();
            memory.MemoryType = DetectMemoryType();
        }
        catch
        {
        }

        return memory;
    }

    private List<StorageDriveInfo> ProfileStorage()
    {
        return _storageDetector.DetectAll();
    }

    private List<StorageDriveInfo> ProfileStorageQuick()
    {
        return _storageDetector.DetectAll();
    }

    private List<MonitorInfoExtended> ProfileMonitors()
    {
        return _monitorDetector.DetectAll();
    }

    private List<MonitorInfoExtended> ProfileMonitorsQuick()
    {
        try
        {
            return _monitorDetector.DetectAll();
        }
        catch
        {
            return new List<MonitorInfoExtended>();
        }
    }

    private MotherboardInfo ProfileMotherboard()
    {
        var mb = new MotherboardInfo();

        try
        {
            using var sysInfo = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            mb.Manufacturer = sysInfo?.GetValue("SystemManufacturer")?.ToString();
            mb.Model = sysInfo?.GetValue("SystemProductName")?.ToString();
            mb.BiosVendor = sysInfo?.GetValue("BIOSVendor")?.ToString();
            mb.BiosVersion = sysInfo?.GetValue("BIOSVersion")?.ToString();
            
            var biosDate = sysInfo?.GetValue("BIOSReleaseDate")?.ToString();
            if (!string.IsNullOrWhiteSpace(biosDate) && DateTime.TryParse(biosDate, out var date))
            {
                mb.BiosDate = date;
            }

            using var baseBoard = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            mb.Chipset = baseBoard?.GetValue("BaseBoardProduct")?.ToString();
            mb.FormFactor = baseBoard?.GetValue("BaseBoardProduct")?.ToString();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SystemProfiler Motherboard error: {ex.Message}");
        }

        return mb;
    }

    private List<NetworkAdapterInfo> ProfileNetwork()
    {
        var adapters = new List<NetworkAdapterInfo>();

        try
        {
            var psi = new ProcessStartInfo("powershell", "-NoProfile -ExecutionPolicy Bypass -Command \"Get-NetAdapter | Where-Object {$_.Status -eq 'Up'} | Select-Object Name, InterfaceDescription, MacAddress, LinkSpeed | ConvertTo-Json -Depth 2\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            if (process != null)
            {
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrWhiteSpace(output) && output.Contains("Name"))
                {
                    var doc = System.Text.Json.JsonDocument.Parse(output);
                    var root = doc.RootElement;

                    if (root.ValueKind == System.Text.Json.JsonValueKind.Array)
                    {
                        foreach (var element in root.EnumerateArray())
                        {
                            adapters.Add(ParseNetworkAdapter(element));
                        }
                    }
                    else if (root.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        adapters.Add(ParseNetworkAdapter(root));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SystemProfiler Network error: {ex.Message}");
        }

        return adapters;
    }

    private NetworkAdapterInfo ParseNetworkAdapter(System.Text.Json.JsonElement element)
    {
        var name = GetString(element, "Name");
        var desc = GetString(element, "InterfaceDescription");
        
        var speed = GetString(element, "LinkSpeed");
        int? speedMbps = null;
        if (!string.IsNullOrWhiteSpace(speed) && speed.Contains("Gbps"))
        {
            if (double.TryParse(speed.Replace("Gbps", "").Trim(), out var gbps))
                speedMbps = (int)(gbps * 1000);
        }
        else if (!string.IsNullOrWhiteSpace(speed) && speed.Contains("Mbps"))
        {
            if (int.TryParse(speed.Replace("Mbps", "").Trim(), out var mbps))
                speedMbps = mbps;
        }

        return new NetworkAdapterInfo
        {
            Name = name,
            Description = desc,
            Type = desc?.Contains("Wi-Fi", StringComparison.OrdinalIgnoreCase) == true ? "Wi-Fi" : "Ethernet",
            InterfaceType = desc?.Contains("Wi-Fi", StringComparison.OrdinalIgnoreCase) == true ? "802.11ax" : "Ethernet",
            SpeedMbps = speedMbps,
            MacAddress = GetString(element, "MacAddress"),
            IsConnected = true
        };
    }

    private PowerInfo ProfilePower()
    {
        var power = new PowerInfo();

        try
        {
            power.IsLaptop = IsLaptop();
            power.HasBattery = HasBattery();
            
            if (power.HasBattery)
            {
                power.BatteryPercent = GetBatteryPercent();
                power.IsCharging = IsCharging();
            }

            power.PowerPlan = GetPowerPlan(out var planGuid);
            power.PowerPlanGuid = planGuid;
            power.ChassisType = GetChassisType();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SystemProfiler Power error: {ex.Message}");
        }

        return power;
    }

    private PowerInfo ProfilePowerQuick()
    {
        var power = new PowerInfo();

        try
        {
            power.IsLaptop = IsLaptop();
            power.HasBattery = HasBattery();
            power.PowerPlan = GetPowerPlan(out _);
        }
        catch
        {
        }

        return power;
    }

    private int GetPhysicalCoreCount()
    {
        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo("powershell", "-NoProfile -ExecutionPolicy Bypass -Command \"(Get-CimInstance Win32_Processor).NumberOfCores\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            if (int.TryParse(output, out var cores))
                return cores;
        }
        catch
        {
        }

        return Environment.ProcessorCount;
    }

    private string? DetectCpuVendor(string cpuName)
    {
        if (string.IsNullOrWhiteSpace(cpuName))
            return null;

        var lower = cpuName.ToLowerInvariant();
        if (lower.Contains("intel") || lower.Contains("core"))
            return "Intel";
        if (lower.Contains("amd") || lower.Contains("ryzen") || lower.Contains("athlon"))
            return "AMD";
        
        return null;
    }

    private string? DetectCpuArchitecture(string cpuName, string? vendor)
    {
        if (string.IsNullOrWhiteSpace(cpuName))
            return null;

        var lower = cpuName.ToLowerInvariant();

        if (vendor == "Intel")
        {
            if (lower.Contains("i9-14") || lower.Contains("i7-14") || lower.Contains("i5-14") || lower.Contains("i3-14"))
                return "Raptor Lake";
            if (lower.Contains("i9-13") || lower.Contains("i7-13") || lower.Contains("i5-13") || lower.Contains("i3-13"))
                return "Raptor Lake";
            if (lower.Contains("i9-12") || lower.Contains("i7-12") || lower.Contains("i5-12") || lower.Contains("i3-12"))
                return "Alder Lake";
            if (lower.Contains("i9-11") || lower.Contains("i7-11") || lower.Contains("i5-11") || lower.Contains("i3-11"))
                return "Rocket Lake";
        }
        else if (vendor == "AMD")
        {
            if (lower.Contains("ryzen 9 7") || lower.Contains("ryzen 7 7") || lower.Contains("ryzen 5 7"))
                return "Zen 4 (Raphael)";
            if (lower.Contains("ryzen 9 5") || lower.Contains("ryzen 7 5") || lower.Contains("ryzen 5 5"))
                return "Zen 3 (Vermeer)";
            if (lower.Contains("ryzen 9 4") || lower.Contains("ryzen 7 4") || lower.Contains("ryzen 5 4"))
                return "Zen 2 (Matisse)";
        }

        return null;
    }

    private string? DetectCpuGeneration(string cpuName, string? vendor)
    {
        if (string.IsNullOrWhiteSpace(cpuName))
            return null;

        var lower = cpuName.ToLowerInvariant();

        if (vendor == "Intel")
        {
            if (lower.Contains("-14"))
                return "14th Gen";
            if (lower.Contains("-13"))
                return "13th Gen";
            if (lower.Contains("-12"))
                return "12th Gen";
            if (lower.Contains("-11"))
                return "11th Gen";
            if (lower.Contains("-10"))
                return "10th Gen";
        }
        else if (vendor == "AMD")
        {
            if (lower.Contains("7000"))
                return "Ryzen 7000";
            if (lower.Contains("5000"))
                return "Ryzen 5000";
            if (lower.Contains("3000"))
                return "Ryzen 3000";
        }

        return null;
    }

    private bool IsHybridCpu(string cpuName)
    {
        if (string.IsNullOrWhiteSpace(cpuName))
            return false;

        var lower = cpuName.ToLowerInvariant();
        return lower.Contains("hybrid") || (lower.Contains("intel") && lower.Contains("p-core"));
    }

    private int EstimateCpuTdp(string? cpuName)
    {
        if (string.IsNullOrWhiteSpace(cpuName))
            return 65;

        var lower = cpuName.ToLowerInvariant();
        
        if (lower.Contains("i9") || lower.Contains("ryzen 9"))
            return 125;
        if (lower.Contains("i7") || lower.Contains("ryzen 7"))
            return 105;
        if (lower.Contains("i5") || lower.Contains("ryzen 5"))
            return 65;
        if (lower.Contains("i3") || lower.Contains("ryzen 3"))
            return 35;
        
        if (lower.Contains("mobile") || lower.Contains("u-") || lower.Contains("u "))
            return 15;

        return 65;
    }

    private bool IsMobileCpu(string? cpuName)
    {
        if (string.IsNullOrWhiteSpace(cpuName))
            return false;

        var lower = cpuName.ToLowerInvariant();
        return lower.Contains("mobile") || lower.Contains("u-") || lower.Contains("u ") || 
               lower.Contains("hq") || lower.Contains("hk") || lower.Contains("h ");
    }

    private string? DetectCpuSocket(string? cpuName)
    {
        if (string.IsNullOrWhiteSpace(cpuName))
            return null;

        var lower = cpuName.ToLowerInvariant();
        
        if (lower.Contains("1700"))
            return "LGA1700";
        if (lower.Contains("1200"))
            return "LGA1200";
        if (lower.Contains("1151"))
            return "LGA1151";
        if (lower.Contains("am5"))
            return "AM5";
        if (lower.Contains("am4"))
            return "AM4";
        
        return "Unknown";
    }

    private int GetMemorySpeed()
    {
        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo("powershell", "-NoProfile -ExecutionPolicy Bypass -Command \"(Get-CimInstance Win32_PhysicalMemory | Measure-Object -Property Speed -Average).Average\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            if (int.TryParse(output, out var speed))
                return speed;
        }
        catch
        {
        }

        return 0;
    }

    private string? DetectMemoryType()
    {
        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo("powershell", "-NoProfile -ExecutionPolicy Bypass -Command \"(Get-CimInstance Win32_PhysicalMemory | Select-Object -First 1).SMBIOSMemoryType\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            if (int.TryParse(output, out var type))
            {
                return type switch
                {
                    20 => "DDR",
                    21 => "DDR2",
                    24 => "DDR3",
                    26 => "DDR4",
                    34 => "DDR5",
                    _ => "Unknown"
                };
            }
        }
        catch
        {
        }

        return "DDR4";
    }

    private int GetMemorySlotsTotal()
    {
        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo("powershell", "-NoProfile -ExecutionPolicy Bypass -Command \"(Get-CimInstance Win32_PhysicalMemory | Measure-Object).Count\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            if (int.TryParse(output, out var slots))
                return slots;
        }
        catch
        {
        }

        return 2;
    }

    private int GetMemorySlotsUsed()
    {
        return GetMemorySlotsTotal();
    }

    private List<MemoryModuleInfo> GetMemoryModules()
    {
        var modules = new List<MemoryModuleInfo>();

        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo("powershell", "-NoProfile -ExecutionPolicy Bypass -Command \"Get-CimInstance Win32_PhysicalMemory | Select-Object Manufacturer, PartNumber, Capacity, Speed, SMBIOSMemoryType, BankLabel, DeviceLocator | ConvertTo-Json -Depth 2\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(output) && output.Contains("Manufacturer"))
            {
                var doc = System.Text.Json.JsonDocument.Parse(output);
                var root = doc.RootElement;

                if (root.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    foreach (var element in root.EnumerateArray())
                    {
                        modules.Add(ParseMemoryModule(element));
                    }
                }
                else if (root.ValueKind == System.Text.Json.JsonValueKind.Object)
                {
                    modules.Add(ParseMemoryModule(root));
                }
            }
        }
        catch
        {
        }

        return modules;
    }

    private MemoryModuleInfo ParseMemoryModule(System.Text.Json.JsonElement element)
    {
        var capacity = GetUlong(element, "Capacity");
        var bankStr = GetString(element, "BankLabel");
        var slotStr = GetString(element, "DeviceLocator");
        
        return new MemoryModuleInfo
        {
            Manufacturer = GetString(element, "Manufacturer"),
            PartNumber = GetString(element, "PartNumber"),
            CapacityBytes = capacity,
            SpeedMhz = GetInt(element, "Speed"),
            MemoryType = GetInt(element, "SMBIOSMemoryType") switch
            {
                24 => "DDR3",
                26 => "DDR4",
                34 => "DDR5",
                _ => "Unknown"
            },
            Bank = bankStr != null ? bankStr.Length : 0,
            Slot = slotStr != null ? slotStr.Length : 0
        };
    }

    private bool IsLaptop()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            var chassisType = key?.GetValue("SystemEnclosure")?.ToString();
            
            if (!string.IsNullOrWhiteSpace(chassisType) && int.TryParse(chassisType, out var type))
            {
                return type == 10 || type == 9 || type == 8 || type == 6 || type == 5;
            }
        }
        catch
        {
        }

        return false;
    }

    private bool HasBattery()
    {
        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo("powershell", "-NoProfile -ExecutionPolicy Bypass -Command \"(Get-CimInstance Win32_Battery -ErrorAction SilentlyContinue)\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return !string.IsNullOrWhiteSpace(output) && output.Contains("Battery");
        }
        catch
        {
        }

        return false;
    }

    private int? GetBatteryPercent()
    {
        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo("powershell", "-NoProfile -ExecutionPolicy Bypass -Command \"(Get-CimInstance Win32_Battery -ErrorAction SilentlyContinue).EstimatedChargeRemaining\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            if (int.TryParse(output, out var percent))
                return percent;
        }
        catch
        {
        }

        return null;
    }

    private bool IsCharging()
    {
        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo("powershell", "-NoProfile -ExecutionPolicy Bypass -Command \"(Get-CimInstance Win32_Battery -ErrorAction SilentlyContinue).BatteryStatus\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            return output == "2";
        }
        catch
        {
        }

        return false;
    }

    private string? GetPowerPlan(out string? planGuid)
    {
        planGuid = null;
        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo("powershell", "-NoProfile -ExecutionPolicy Bypass -Command \"(Get-CimInstance Win32_PowerPlan -Namespace root\\cimv2\\power -ErrorAction SilentlyContinue | Where-Object {$_.IsActive -eq $true}).InstanceID\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            if (output.Contains("381b4222"))
            {
                planGuid = output;
                return "Balanced";
            }
            if (output.Contains("8c5e7fda"))
            {
                planGuid = output;
                return "High Performance";
            }
            if (output.Contains("a1841308"))
            {
                planGuid = output;
                return "Power Saver";
            }
            if (output.Contains("e9a42b02"))
            {
                planGuid = output;
                return "Ultimate Performance";
            }

            planGuid = output;
            return "Custom";
        }
        catch
        {
        }

        return "Unknown";
    }

    private string? GetChassisType()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            var chassisType = key?.GetValue("SystemEnclosure")?.ToString();
            
            if (!string.IsNullOrWhiteSpace(chassisType) && int.TryParse(chassisType, out var type))
            {
                return type switch
                {
                    1 => "Other",
                    2 => "Unknown",
                    3 => "Desktop",
                    4 => "Low Profile Desktop",
                    5 => "Pizza Box",
                    6 => "Mini Tower",
                    7 => "Tower",
                    8 => "Portable",
                    9 => "Laptop",
                    10 => "Notebook",
                    11 => "All-in-One",
                    12 => "Sub notebook",
                    13 => "Space-saving",
                    14 => "Lunch box",
                    15 => "Hand held",
                    _ => "Unknown"
                };
            }
        }
        catch
        {
        }

        return "Unknown";
    }

    private static string? GetString(System.Text.Json.JsonElement element, string property)
    {
        if (!element.TryGetProperty(property, out var value))
            return null;
        return value.ValueKind == System.Text.Json.JsonValueKind.String ? value.GetString() : value.ToString();
    }

    private static int? GetInt(System.Text.Json.JsonElement element, string property)
    {
        if (!element.TryGetProperty(property, out var value))
            return null;
        if (value.ValueKind == System.Text.Json.JsonValueKind.Number && value.TryGetInt32(out var numeric))
            return numeric;
        return null;
    }

    private static ulong? GetUlong(System.Text.Json.JsonElement element, string property)
    {
        if (!element.TryGetProperty(property, out var value))
            return null;
        if (value.ValueKind == System.Text.Json.JsonValueKind.Number && value.TryGetUInt64(out var numeric))
            return numeric;
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
        }
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool GlobalMemoryStatusEx(ref MemoryStatusEx lpBuffer);
}

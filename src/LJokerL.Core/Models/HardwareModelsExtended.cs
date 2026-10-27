using System;
using System.Collections.Generic;

namespace LJokerL.Core.Models;

public sealed class ExtendedHardwareSnapshot
{
    public DateTime DetectedAtUtc { get; set; }
    public CpuInfoExtended Cpu { get; set; } = new();
    public List<GpuInfoExtended> Gpus { get; set; } = new();
    public GpuInfoExtended? PrimaryGpu => Gpus.Count > 0 ? Gpus[0] : null;
    public MemoryInfoExtended Memory { get; set; } = new();
    public List<StorageDriveInfo> Storage { get; set; } = new();
    public StorageDriveInfo? SystemDrive => Storage.Count > 0 ? Storage[0] : null;
    public List<MonitorInfoExtended> Monitors { get; set; } = new();
    public MonitorInfoExtended? PrimaryMonitor => Monitors.Count > 0 ? Monitors[0] : null;
    public MotherboardInfo Motherboard { get; set; } = new();
    public List<NetworkAdapterInfo> NetworkAdapters { get; set; } = new();
    public NetworkAdapterInfo? PrimaryNetwork => NetworkAdapters.Count > 0 ? NetworkAdapters[0] : null;
    public PowerInfo Power { get; set; } = new();
    public SoftwareEnvironmentInfo Software { get; set; } = new();
}

public sealed class CpuInfoExtended
{
    public string? Name { get; set; }
    public string? Manufacturer { get; set; }
    public string? Vendor { get; set; }
    public string? Architecture { get; set; }
    public string? Generation { get; set; }
    public int? PhysicalCores { get; set; }
    public int? LogicalProcessors { get; set; }
    public int? PerformanceCores { get; set; }
    public int? EfficiencyCores { get; set; }
    public int? MaxClockMhz { get; set; }
    public int? BaseClockMhz { get; set; }
    public int? TdpWatts { get; set; }
    public int? TemperatureCelsius { get; set; }
    public int? L1CacheKb { get; set; }
    public int? L2CacheKb { get; set; }
    public int? L3CacheKb { get; set; }
    public string? Socket { get; set; }
    public bool IsHybrid { get; set; }
    public bool IsMobile { get; set; }
    public string? BiosVersion { get; set; }
}

public sealed class GpuInfoExtended
{
    public string? Name { get; set; }
    public string? Vendor { get; set; }
    public string? Model { get; set; }
    public ulong? VramBytes { get; set; }
    public string? VramType { get; set; }
    public int? CoreClockMhz { get; set; }
    public int? MemoryClockMhz { get; set; }
    public int? TemperatureCelsius { get; set; }
    public string? DriverVersion { get; set; }
    public DateTime? DriverDate { get; set; }
    public string? DirectXVersion { get; set; }
    public string? VulkanVersion { get; set; }
    public bool IsIntegrated { get; set; }
    public bool IsActive { get; set; }
    public int? PowerLimitWatts { get; set; }
    public int? UtilizationPercent { get; set; }
    public string? Architecture { get; set; }
}

public sealed class MemoryInfoExtended
{
    public ulong? TotalBytes { get; set; }
    public int? SpeedMhz { get; set; }
    public string? MemoryType { get; set; }
    public int? SlotsUsed { get; set; }
    public int? SlotsTotal { get; set; }
    public bool IsDualChannel { get; set; }
    public List<MemoryModuleInfo> Modules { get; set; } = new();
}

public sealed class MemoryModuleInfo
{
    public string? Manufacturer { get; set; }
    public string? PartNumber { get; set; }
    public ulong? CapacityBytes { get; set; }
    public int? SpeedMhz { get; set; }
    public string? MemoryType { get; set; }
    public int? Bank { get; set; }
    public int? Slot { get; set; }
}

public sealed class StorageDriveInfo
{
    public string? DriveLetter { get; set; }
    public string? VolumeLabel { get; set; }
    public string? MediaType { get; set; }
    public string? InterfaceType { get; set; }
    public ulong? TotalBytes { get; set; }
    public ulong? FreeBytes { get; set; }
    public double UsagePercent { get; set; }
    public int? ReadSpeedMBs { get; set; }
    public int? WriteSpeedMBs { get; set; }
    public int? HealthPercent { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public bool IsSystemDrive { get; set; }
    public bool IsSsd { get; set; }
    public bool IsNvme { get; set; }
}

public sealed class MonitorInfoExtended
{
    public string? Name { get; set; }
    public string? DeviceId { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? RefreshRateHz { get; set; }
    public double? RefreshRateDecimal { get; set; }
    public bool? VrrSupported { get; set; }
    public string? VrrType { get; set; }
    public bool? IsHdrCapable { get; set; }
    public string? HDRFormat { get; set; }
    public int? ColorDepth { get; set; }
    public string? ColorSpace { get; set; }
    public bool IsPrimary { get; set; }
    public int? PositionX { get; set; }
    public int? PositionY { get; set; }
    public double? SizeInches { get; set; }
    public string? Manufacturer { get; set; }
}

public sealed class MotherboardInfo
{
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? BiosVendor { get; set; }
    public string? BiosVersion { get; set; }
    public DateTime? BiosDate { get; set; }
    public string? Chipset { get; set; }
    public string? Southbridge { get; set; }
    public string? SlotCount { get; set; }
    public string? FormFactor { get; set; }
}

public sealed class NetworkAdapterInfo
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public string? InterfaceType { get; set; }
    public int? SpeedMbps { get; set; }
    public string? MacAddress { get; set; }
    public bool IsConnected { get; set; }
    public string? IpAddress { get; set; }
    public string? Gateway { get; set; }
    public List<string> DnsServers { get; set; } = new();
    public string? DhcpServer { get; set; }
    public bool DhcpEnabled { get; set; }
}

public sealed class PowerInfo
{
    public bool IsLaptop { get; set; }
    public bool HasBattery { get; set; }
    public int? BatteryPercent { get; set; }
    public bool IsCharging { get; set; }
    public string? PowerPlan { get; set; }
    public string? PowerPlanGuid { get; set; }
    public int? PsuWatts { get; set; }
    public string? ChassisType { get; set; }
}

public sealed class SoftwareEnvironmentInfo
{
    public string? WindowsVersion { get; set; }
    public string? WindowsBuild { get; set; }
    public string? WindowsEdition { get; set; }
    public string? NetFrameworkVersion { get; set; }
    public string? DirectXVersion { get; set; }
    public string? DirectXAgiVersion { get; set; }
    public string? OpenGlVersion { get; set; }
    public string? VulkanVersion { get; set; }
    public string? VcRedistVersions { get; set; }
    public string? WindowsUpdateStatus { get; set; }
    public bool FirewallEnabled { get; set; }
    public string? AntivirusName { get; set; }
    public bool AntivirusEnabled { get; set; }
    public List<GamingAppInfo> GamingApps { get; set; } = new();
    public List<StartupAppInfo> StartupApps { get; set; } = new();
    public List<string> CriticalServices { get; set; } = new();
    public List<string> DisabledServices { get; set; } = new();
    public int RunningProcessCount { get; set; }
    public int GpuProcessCount { get; set; }
}

public sealed class GamingAppInfo
{
    public string? Name { get; set; }
    public string? ExecutablePath { get; set; }
    public string? InstallLocation { get; set; }
    public string? Store { get; set; }
    public bool IsInstalled { get; set; }
}

public sealed class StartupAppInfo
{
    public string? Name { get; set; }
    public string? Command { get; set; }
    public string? Location { get; set; }
    public bool Enabled { get; set; }
    public string? Publisher { get; set; }
    public int? Impact { get; set; }
}

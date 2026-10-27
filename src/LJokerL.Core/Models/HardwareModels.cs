using System;

namespace LJokerL.Core.Models;

public sealed class HardwareSnapshot
{
    public DateTime DetectedAtUtc { get; set; }
    public CpuInfo? Cpu { get; set; }
    public GpuInfo? Gpu { get; set; }
    public MemoryInfo? Memory { get; set; }
    public OsInfo? Os { get; set; }
    public MonitorInfo? Monitor { get; set; }
}

public sealed class CpuInfo
{
    public string? Name { get; set; }
    public string? Manufacturer { get; set; }
    public int? Cores { get; set; }
    public int? LogicalProcessors { get; set; }
    public int? MaxClockMhz { get; set; }
}

public sealed class GpuInfo
{
    public string? Name { get; set; }
    public string? Vendor { get; set; }
    public ulong? VramBytes { get; set; }
}

public sealed class MemoryInfo
{
    public ulong? TotalBytes { get; set; }
    public int? AverageSpeedMhz { get; set; }
}

public sealed class OsInfo
{
    public string? Caption { get; set; }
    public string? Version { get; set; }
    public string? BuildNumber { get; set; }
}

public sealed class MonitorInfo
{
    public string? Name { get; set; }
    public bool? VrrSupported { get; set; }
}

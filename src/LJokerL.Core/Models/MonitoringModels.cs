using System;
using System.Collections.Generic;

namespace LJokerL.Core.Models;

public sealed class SystemMetricsSnapshot
{
    public DateTime TimestampUtc { get; set; }
    public CpuMetrics Cpu { get; set; } = new();
    public GpuMetrics? Gpu { get; set; }
    public MemoryMetrics Memory { get; set; } = new();
    public List<DiskMetrics> Disks { get; set; } = new();
    public NetworkMetrics Network { get; set; } = new();
    public List<ProcessMetrics> Processes { get; set; } = new();
}

public sealed class CpuMetrics
{
    public string? Name { get; set; }
    public double UsagePercent { get; set; }
    public double? TemperatureCelsius { get; set; }
}

public sealed class GpuMetrics
{
    public string? Name { get; set; }
    public double? UsagePercent { get; set; }
    public ulong? VramBytes { get; set; }
}

public sealed class MemoryMetrics
{
    public ulong TotalBytes { get; set; }
    public ulong AvailableBytes { get; set; }
    public double UsagePercent { get; set; }
}

public sealed class DiskMetrics
{
    public string Name { get; set; } = "";
    public ulong TotalBytes { get; set; }
    public ulong FreeBytes { get; set; }
    public double UsagePercent { get; set; }
}

public sealed class NetworkMetrics
{
    public int AdapterCount { get; set; }
    public double BytesReceivedPerSec { get; set; }
    public double BytesSentPerSec { get; set; }
}

public sealed class ProcessMetrics
{
    public string Name { get; set; } = "";
    public int Id { get; set; }
    public double MemoryMb { get; set; }
    public double CpuMinutes { get; set; }
}

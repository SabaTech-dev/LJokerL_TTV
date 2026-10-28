namespace LJokerL.Core.Models;

public sealed class UserSettings
{
    public bool? VrrOverride { get; set; }
    public string? GpuNameOverride { get; set; }
    public string? GpuVendorOverride { get; set; }
    public string? CpuNameOverride { get; set; }
    public int? CpuCoresOverride { get; set; }
    public int? CpuThreadsOverride { get; set; }
    public int? RamTotalGbOverride { get; set; }
    public int? RamSpeedMhzOverride { get; set; }
    public string? OsCaptionOverride { get; set; }
    public string? OsVersionOverride { get; set; }
    public string? OsBuildOverride { get; set; }
    public bool MonitoringEnabled { get; set; } = true;
    public bool MetricsLoggingEnabled { get; set; }
    public int MonitoringIntervalSeconds { get; set; } = 2;
    public bool CreateRestorePointBeforeApply { get; set; } = true;
    public bool DarkMode { get; set; }
}

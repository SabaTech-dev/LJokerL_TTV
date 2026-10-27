using System;
using System.Collections.Generic;

namespace LJokerL.Core.Models;

public sealed class AutoConfigResult
{
    public DateTime GeneratedAtUtc { get; set; }
    public ExtendedHardwareSnapshot Hardware { get; set; } = new();
    public List<AutoConfigRecommendation> Recommendations { get; set; } = new();
    public List<AutoConfigRecommendation> SafeRecommendations { get; set; } = new();
    public List<AutoConfigRecommendation> AdvancedRecommendations { get; set; } = new();
    public List<AutoConfigRecommendation> ExperimentalRecommendations { get; set; } = new();
    public List<AppliedConfigSummary> AppliedConfigs { get; set; } = new();
    public AutoConfigSummary Summary { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public List<string> Errors { get; set; } = new();
}

public sealed class AutoConfigSummary
{
    public int TotalRecommendations { get; set; }
    public int SafeCount { get; set; }
    public int AdvancedCount { get; set; }
    public int ExperimentalCount { get; set; }
    public int AppliedCount { get; set; }
    public string? RecommendedProfile { get; set; }
    public bool RequiresReboot { get; set; }
    public List<string> RebootRequiredReasons { get; set; } = new();
    public TimeSpan EstimatedTime { get; set; }
}

public sealed class AutoConfigRecommendation
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string RiskLevel { get; set; } = string.Empty;
    public bool IsApplied { get; set; }
    public bool RequiresReboot { get; set; }
    public string? RebootReason { get; set; }
    public double ImpactScore { get; set; }
    public List<string> ApplicableConditions { get; set; } = new();
    public List<ConfigAction> Actions { get; set; } = new();
}

public sealed class AppliedConfigSummary
{
    public string? ProfileId { get; set; }
    public string? ProfileName { get; set; }
    public DateTime AppliedAtUtc { get; set; }
    public int TweaksApplied { get; set; }
    public int ServicesModified { get; set; }
    public int AppsRemoved { get; set; }
    public List<string> AppliedTweaks { get; set; } = new();
    public List<string> FailedTweaks { get; set; } = new();
    public string? RollbackContextId { get; set; }
}

public sealed class ConfigAction
{
    public string Type { get; set; } = string.Empty;
    public string Target { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Value { get; set; }
    public string? Operation { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}

public sealed class AutoConfigRule
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RuleCondition Condition { get; set; } = new();
    public RuleAction Action { get; set; } = new();
    public string RiskLevel { get; set; } = "safe";
    public double Priority { get; set; }
    public bool Enabled { get; set; } = true;
    public List<string> Tags { get; set; } = new();
}

public sealed class RuleCondition
{
    public string? CpuVendor { get; set; }
    public string? CpuArchitecture { get; set; }
    public int? CpuCoresMin { get; set; }
    public int? CpuCoresMax { get; set; }
    public int? CpuThreadsMin { get; set; }
    public int? RamGbMin { get; set; }
    public int? RamGbMax { get; set; }
    public string? GpuVendor { get; set; }
    public int? GpuVramMbMin { get; set; }
    public int? GpuVramMbMax { get; set; }
    public bool? MonitorVrr { get; set; }
    public int? MonitorRefreshRateMin { get; set; }
    public bool? IsLaptop { get; set; }
    public bool? IsSsd { get; set; }
    public bool? IsNvme { get; set; }
    public bool? HasBattery { get; set; }
    public string? StorageMediaType { get; set; }
    public string? WindowsVersion { get; set; }
    public int? WindowsBuildMin { get; set; }
    public string? NetworkType { get; set; }
    public bool? HasGamingApps { get; set; }
    public string? PowerPlan { get; set; }
    public bool? IsHybridCpu { get; set; }
    public List<string>? CpuArchitectures { get; set; }
    public List<string>? GpuVendors { get; set; }
}

public sealed class RuleAction
{
    public string Type { get; set; } = string.Empty;
    public Dictionary<string, object> Parameters { get; set; } = new();
    public string? TweakId { get; set; }
    public string? ServiceId { get; set; }
    public string? ProfileId { get; set; }
    public string? RegistryPath { get; set; }
    public string? RegistryName { get; set; }
    public string? RegistryValue { get; set; }
    public string? Command { get; set; }
    public string? Message { get; set; }
    public bool RequiresReboot { get; set; }
}

public sealed class AutoConfigRulesCatalog
{
    public string Version { get; set; } = string.Empty;
    public List<AutoConfigRule> Rules { get; set; } = new();
}

using System.Collections.Generic;
using System.Text.Json;

namespace LJokerL.Core.Models;

public sealed class TweakCatalog
{
    public string Version { get; set; } = "";
    public List<TweakDefinition> Tweaks { get; set; } = new();
}

public sealed class TweakDefinition
{
    public string Id { get; set; } = "";
    public string Category { get; set; } = "";
    public string Risk { get; set; } = "";
    public bool RequiresReboot { get; set; }
    public string Description { get; set; } = "";
    public ConditionsDefinition? Conditions { get; set; }
    public List<ActionDefinition> Actions { get; set; } = new();
    public List<ActionDefinition> Revert { get; set; } = new();
    public List<string> Sources { get; set; } = new();
    public List<string> Profiles { get; set; } = new();
}

public sealed class ConditionsDefinition
{
    public List<string> GpuVendor { get; set; } = new();
    public string? Os { get; set; }
    public List<string> Symptoms { get; set; } = new();
}

public sealed class ActionDefinition
{
    public string Type { get; set; } = "";
    public string? Path { get; set; }
    public string? Name { get; set; }
    public string? ValueType { get; set; }
    public JsonElement? Value { get; set; }
    public string? Operation { get; set; }
    public string? Command { get; set; }
}

public sealed class ServiceCatalog
{
    public string Version { get; set; } = "";
    public List<ServiceDefinition> Services { get; set; } = new();
}

public sealed class ServiceDefinition
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string Action { get; set; } = "";
    public string Risk { get; set; } = "";
    public List<string> Profiles { get; set; } = new();
    public string? Notes { get; set; }
}

public sealed class AppCatalog
{
    public string Version { get; set; } = "";
    public List<AppPackageDefinition> Apps { get; set; } = new();
}

public sealed class AppPackageDefinition
{
    public string Id { get; set; } = "";
    public string PackageName { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string Risk { get; set; } = "";
    public List<string> Profiles { get; set; } = new();
    public string? Notes { get; set; }
}

public sealed class ProfileCatalog
{
    public string Version { get; set; } = "";
    public List<ProfileDefinition> Profiles { get; set; } = new();
}

public sealed class ProfileDefinition
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Tweaks { get; set; } = new();
    public List<string> Services { get; set; } = new();
    public List<string> Apps { get; set; } = new();
}

public sealed class HardwareRuleCatalog
{
    public string Version { get; set; } = "";
    public List<HardwareRuleDefinition> Rules { get; set; } = new();
}

public sealed class HardwareRuleDefinition
{
    public string Id { get; set; } = "";
    public HardwareRuleCondition? When { get; set; }
    public HardwareRuleRecommendation? Recommend { get; set; }
}

public sealed class HardwareRuleCondition
{
    public bool? MonitorVrr { get; set; }
    public int? CpuCoresMin { get; set; }
    public int? CpuCoresMax { get; set; }
    public string? CpuVendor { get; set; }
    public int? CpuGenerationMin { get; set; }
    public int? RamGbMin { get; set; }
    public int? RamGbMax { get; set; }
    public string? GpuVendor { get; set; }
    public int? GpuVramMin { get; set; }
    public int? GpuVramMax { get; set; }
    public bool? HasSsd { get; set; }
}

public sealed class HardwareRuleRecommendation
{
    public int? FpsCapOffset { get; set; }
    public bool? VsyncDriver { get; set; }
    public bool? VsyncInGame { get; set; }
    public bool? DisableBackgroundApps { get; set; }
    public int? PageFileSizeGb { get; set; }
    public string? TextureQuality { get; set; }
    public bool? UsePCoresOnly { get; set; }
    public string? ThreadDirectorMode { get; set; }
    public string? LowLatencyMode { get; set; }
    public string? PowerManagement { get; set; }
    public bool? EnableGameMode { get; set; }
    public bool? DisableFullscreenOptimizations { get; set; }
    public bool? EnableHpet { get; set; }
    public int? TimerResolution { get; set; }
}

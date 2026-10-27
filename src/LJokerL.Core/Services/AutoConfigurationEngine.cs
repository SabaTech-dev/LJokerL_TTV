using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using LJokerL.Core.Models;

namespace LJokerL.Core.Services;

public sealed class AutoConfigurationEngine
{
    private readonly List<AutoConfigRule> _rules = new();

    public AutoConfigurationEngine()
    {
        LoadDefaultRules();
    }

    public AutoConfigurationEngine(string rulesPath)
    {
        LoadRulesFromFile(rulesPath);
    }

    public AutoConfigResult GenerateRecommendations(ExtendedHardwareSnapshot hardware)
    {
        var result = new AutoConfigResult
        {
            GeneratedAtUtc = DateTime.UtcNow,
            Hardware = hardware
        };

        try
        {
            var ramGb = hardware.Memory.TotalBytes.HasValue 
                ? hardware.Memory.TotalBytes.Value / (1024.0 * 1024 * 1024) 
                : 0;

            var gpuVramMb = hardware.PrimaryGpu?.VramBytes.HasValue == true 
                ? (long)(hardware.PrimaryGpu.VramBytes.Value / (1024 * 1024)) 
                : 0L;

            foreach (var rule in _rules.Where(r => r.Enabled))
            {
                if (EvaluateCondition(rule.Condition, hardware, ramGb, gpuVramMb))
                {
                    var recommendation = CreateRecommendation(rule, hardware);
                    result.Recommendations.Add(recommendation);
                    
                    if (rule.RiskLevel == "safe")
                        result.SafeRecommendations.Add(recommendation);
                    else if (rule.RiskLevel == "advanced")
                        result.AdvancedRecommendations.Add(recommendation);
                    else
                        result.ExperimentalRecommendations.Add(recommendation);
                }
            }

            result.Summary.TotalRecommendations = result.Recommendations.Count;
            result.Summary.SafeCount = result.SafeRecommendations.Count;
            result.Summary.AdvancedCount = result.AdvancedRecommendations.Count;
            result.Summary.ExperimentalCount = result.ExperimentalRecommendations.Count;
            result.Summary.RequiresReboot = result.Recommendations.Any(r => r.RequiresReboot);
            result.Summary.RebootRequiredReasons = result.Recommendations
                .Where(r => r.RequiresReboot)
                .Select(r => r.RebootReason)
                .Where(r => !string.IsNullOrEmpty(r))
                .Cast<string>()
                .ToList();
            
            result.Summary.RecommendedProfile = DetermineRecommendedProfile(hardware, ramGb);
            result.Summary.EstimatedTime = TimeSpan.FromSeconds(result.Recommendations.Count * 0.5);
        }
        catch (Exception ex)
        {
            result.Errors.Add($"Error generating recommendations: {ex.Message}");
            Debug.WriteLine($"AutoConfigurationEngine error: {ex}");
        }

        return result;
    }

    private bool EvaluateCondition(RuleCondition condition, ExtendedHardwareSnapshot hardware, double ramGb, long gpuVramMb)
    {
        if (condition.CpuVendor != null && hardware.Cpu.Vendor != condition.CpuVendor)
            return false;

        if (!string.IsNullOrEmpty(condition.CpuArchitecture) && hardware.Cpu.Architecture != condition.CpuArchitecture)
            return false;

        if (condition.CpuCoresMin.HasValue && (!hardware.Cpu.PhysicalCores.HasValue || hardware.Cpu.PhysicalCores.Value < condition.CpuCoresMin.Value))
            return false;

        if (condition.CpuCoresMax.HasValue && (!hardware.Cpu.PhysicalCores.HasValue || hardware.Cpu.PhysicalCores.Value > condition.CpuCoresMax.Value))
            return false;

        if (condition.CpuThreadsMin.HasValue && (!hardware.Cpu.LogicalProcessors.HasValue || hardware.Cpu.LogicalProcessors.Value < condition.CpuThreadsMin.Value))
            return false;

        if (condition.RamGbMin.HasValue && ramGb < condition.RamGbMin.Value)
            return false;

        if (condition.RamGbMax.HasValue && ramGb > condition.RamGbMax.Value)
            return false;

        if (!string.IsNullOrEmpty(condition.GpuVendor) && hardware.PrimaryGpu?.Vendor != condition.GpuVendor)
            return false;

        if (condition.GpuVramMbMin.HasValue && gpuVramMb < condition.GpuVramMbMin.Value)
            return false;

        if (condition.GpuVramMbMax.HasValue && gpuVramMb > condition.GpuVramMbMax.Value)
            return false;

        if (condition.MonitorVrr.HasValue && hardware.PrimaryMonitor?.VrrSupported != condition.MonitorVrr.Value)
            return false;

        if (condition.MonitorRefreshRateMin.HasValue)
        {
            var refreshRate = hardware.PrimaryMonitor?.RefreshRateHz;
            if (!refreshRate.HasValue || refreshRate.Value < condition.MonitorRefreshRateMin.Value)
                return false;
        }

        if (condition.IsLaptop.HasValue && hardware.Power.IsLaptop != condition.IsLaptop.Value)
            return false;

        if (condition.IsSsd.HasValue && hardware.SystemDrive?.IsSsd != condition.IsSsd.Value)
            return false;

        if (condition.IsNvme.HasValue && hardware.SystemDrive?.IsNvme != condition.IsNvme.Value)
            return false;

        if (condition.HasBattery.HasValue && hardware.Power.HasBattery != condition.HasBattery.Value)
            return false;

        if (!string.IsNullOrEmpty(condition.StorageMediaType) && hardware.SystemDrive?.MediaType != condition.StorageMediaType)
            return false;

        if (condition.IsHybridCpu.HasValue && hardware.Cpu.IsHybrid != condition.IsHybridCpu.Value)
            return false;

        if (!string.IsNullOrEmpty(condition.PowerPlan) && hardware.Power.PowerPlan != condition.PowerPlan)
            return false;

        if (condition.HasGamingApps.HasValue)
        {
            var hasGamingApps = hardware.Software?.GamingApps?.Count > 0;
            if (hasGamingApps != condition.HasGamingApps.Value)
                return false;
        }

        return true;
    }

    private AutoConfigRecommendation CreateRecommendation(AutoConfigRule rule, ExtendedHardwareSnapshot hardware)
    {
        var recommendation = new AutoConfigRecommendation
        {
            Id = rule.Id,
            Name = rule.Name,
            Description = rule.Description,
            Category = rule.Action.Type,
            RiskLevel = rule.RiskLevel,
            RequiresReboot = rule.Action.RequiresReboot,
            RebootReason = rule.Action.RequiresReboot ? rule.Description : null,
            ImpactScore = rule.Priority
        };

        recommendation.Actions.Add(new ConfigAction
        {
            Type = rule.Action.Type,
            Target = rule.Action.TweakId ?? rule.Action.ServiceId ?? rule.Action.ProfileId ?? "",
            Name = rule.Action.RegistryName,
            Value = rule.Action.RegistryValue,
            Operation = "apply"
        });

        if (!string.IsNullOrEmpty(rule.Action.Message))
        {
            recommendation.Description = rule.Action.Message;
        }

        return recommendation;
    }

    private string DetermineRecommendedProfile(ExtendedHardwareSnapshot hardware, double ramGb)
    {
        if (hardware.Power.IsLaptop)
        {
            return "laptop_saver";
        }

        if (ramGb <= 8)
        {
            return "low_resources";
        }

        if (HasGamingSignals(hardware))
        {
            if (hardware.PrimaryMonitor?.VrrSupported == true)
            {
                return "smooth_vrr";
            }

            return "competitive_latency";
        }

        return "balanced";
    }

    private static bool HasGamingSignals(ExtendedHardwareSnapshot hardware)
    {
        var hasGamingApps = hardware.Software?.GamingApps?.Count > 0;
        var gpuName = hardware.PrimaryGpu?.Name?.ToLowerInvariant() ?? string.Empty;
        var hasGamingGpu = gpuName.Contains("rtx")
            || gpuName.Contains("gtx")
            || gpuName.Contains("radeon")
            || gpuName.Contains("rx ")
            || gpuName.Contains("arc");
        var hasHighRefreshMonitor = hardware.PrimaryMonitor?.RefreshRateHz >= 120;

        return hasGamingApps || hasGamingGpu || hasHighRefreshMonitor;
    }

    private void LoadDefaultRules()
    {
        _rules.Clear();

        _rules.Add(new AutoConfigRule
        {
            Id = "low_ram_profile",
            Name = "Low Resource Profile",
            Description = "Apply the low resource profile for systems with 8GB RAM or less",
            RiskLevel = "safe",
            Priority = 10,
            Condition = new RuleCondition { RamGbMax = 8 },
            Action = new RuleAction { Type = "profile", ProfileId = "low_resources", RequiresReboot = false }
        });

        _rules.Add(new AutoConfigRule
        {
            Id = "laptop_power",
            Name = "Laptop Power Management",
            Description = "Configure power settings for laptops",
            RiskLevel = "safe",
            Priority = 9,
            Condition = new RuleCondition { IsLaptop = true },
            Action = new RuleAction { Type = "profile", ProfileId = "laptop_saver", RequiresReboot = false }
        });

        _rules.Add(new AutoConfigRule
        {
            Id = "smooth_vrr_profile",
            Name = "Smooth VRR Gaming Profile",
            Description = "Apply the Smooth VRR profile for high refresh VRR displays",
            RiskLevel = "safe",
            Priority = 9,
            Condition = new RuleCondition { IsLaptop = false, MonitorVrr = true, MonitorRefreshRateMin = 120 },
            Action = new RuleAction { Type = "profile", ProfileId = "smooth_vrr", RequiresReboot = false }
        });

        _rules.Add(new AutoConfigRule
        {
            Id = "competitive_profile",
            Name = "Competitive Gaming Profile",
            Description = "Apply the Competitive Latency profile for desktop gaming systems without VRR",
            RiskLevel = "safe",
            Priority = 9,
            Condition = new RuleCondition { IsLaptop = false, HasGamingApps = true, MonitorVrr = false },
            Action = new RuleAction { Type = "profile", ProfileId = "competitive_latency", RequiresReboot = false }
        });

        _rules.Add(new AutoConfigRule
        {
            Id = "enable_game_mode",
            Name = "Enable Game Mode",
            Description = "Enable Game Mode for gaming workloads",
            RiskLevel = "safe",
            Priority = 7,
            Condition = new RuleCondition { HasGamingApps = true },
            Action = new RuleAction { Type = "tweak", TweakId = "game_mode_on", RequiresReboot = false }
        });

        _rules.Add(new AutoConfigRule
        {
            Id = "disable_game_dvr",
            Name = "Disable Game DVR",
            Description = "Disable Game DVR for lower latency",
            RiskLevel = "safe",
            Priority = 7,
            Condition = new RuleCondition { HasGamingApps = true },
            Action = new RuleAction { Type = "tweak", TweakId = "game_dvr_off", RequiresReboot = false }
        });

        _rules.Add(new AutoConfigRule
        {
            Id = "enable_hags_nvidia",
            Name = "Enable HAGS for NVIDIA",
            Description = "Enable Hardware Accelerated GPU Scheduling on supported NVIDIA GPUs",
            RiskLevel = "advanced",
            Priority = 8,
            Condition = new RuleCondition { GpuVendor = "NVIDIA" },
            Action = new RuleAction { Type = "tweak", TweakId = "hags_on", RequiresReboot = true }
        });

        _rules.Add(new AutoConfigRule
        {
            Id = "enable_hags_amd",
            Name = "Enable HAGS for AMD",
            Description = "Enable Hardware Accelerated GPU Scheduling on supported AMD GPUs",
            RiskLevel = "advanced",
            Priority = 8,
            Condition = new RuleCondition { GpuVendor = "AMD" },
            Action = new RuleAction { Type = "tweak", TweakId = "hags_on", RequiresReboot = true }
        });

        _rules.Add(new AutoConfigRule
        {
            Id = "disable_mouse_acceleration",
            Name = "Disable Mouse Acceleration",
            Description = "Disable mouse acceleration for more consistent input response",
            RiskLevel = "safe",
            Priority = 6,
            Condition = new RuleCondition { HasGamingApps = true },
            Action = new RuleAction { Type = "tweak", TweakId = "mouse_accel_off", RequiresReboot = false }
        });

        _rules.Add(new AutoConfigRule
        {
            Id = "disable_usb_selective_suspend",
            Name = "Disable USB Selective Suspend",
            Description = "Disable USB selective suspend for desktop gaming peripherals",
            RiskLevel = "advanced",
            Priority = 6,
            Condition = new RuleCondition { HasGamingApps = true, IsLaptop = false },
            Action = new RuleAction { Type = "tweak", TweakId = "usb_selective_suspend_off", RequiresReboot = false }
        });

        _rules.Add(new AutoConfigRule
        {
            Id = "disable_telemetry",
            Name = "Reduce Telemetry",
            Description = "Reduce Windows telemetry for better performance",
            RiskLevel = "safe",
            Priority = 5,
            Condition = new RuleCondition(),
            Action = new RuleAction { Type = "tweak", TweakId = "telemetry_min", RequiresReboot = false }
        });

        _rules.Add(new AutoConfigRule
        {
            Id = "win32_priority_separation",
            Name = "Win32 Priority Separation",
            Description = "Optimize task scheduler responsiveness for games",
            RiskLevel = "advanced",
            Priority = 8,
            Condition = new RuleCondition { HasGamingApps = true, IsLaptop = false },
            Action = new RuleAction { Type = "tweak", TweakId = "win32_priority_separation", RequiresReboot = true }
        });

        _rules.Add(new AutoConfigRule
        {
            Id = "network_throttling_disabled",
            Name = "Disable Network Throttling",
            Description = "Disable network throttling index to prevent ping stutters",
            RiskLevel = "advanced",
            Priority = 7,
            Condition = new RuleCondition { HasGamingApps = true },
            Action = new RuleAction { Type = "tweak", TweakId = "network_throttling_disabled", RequiresReboot = true }
        });

        _rules.Add(new AutoConfigRule
        {
            Id = "multimedia_gpu_priority",
            Name = "Multimedia GPU Priority",
            Description = "Optimize MMCSS Games task priority for high GPU performance",
            RiskLevel = "advanced",
            Priority = 8,
            Condition = new RuleCondition { HasGamingApps = true },
            Action = new RuleAction { Type = "tweak", TweakId = "multimedia_gpu_priority", RequiresReboot = true }
        });

        _rules.Add(new AutoConfigRule
        {
            Id = "allow_game_dvr_policy",
            Name = "Force Disable Game DVR Policy",
            Description = "Prevent background recording overhead using Group Policy overrides",
            RiskLevel = "advanced",
            Priority = 7,
            Condition = new RuleCondition { HasGamingApps = true },
            Action = new RuleAction { Type = "tweak", TweakId = "allow_game_dvr_policy", RequiresReboot = false }
        });
    }

    private void LoadRulesFromFile(string path)
    {
        try
        {
            if (!System.IO.File.Exists(path))
            {
                LoadDefaultRules();
                return;
            }

            var json = System.IO.File.ReadAllText(path);
            var catalog = JsonSerializer.Deserialize<AutoConfigRulesCatalog>(json);
            
            if (catalog?.Rules != null && catalog.Rules.Count > 0)
            {
                _rules.Clear();
                _rules.AddRange(catalog.Rules);
            }
            else
            {
                LoadDefaultRules();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading rules from {path}: {ex.Message}");
            LoadDefaultRules();
        }
    }

    public void AddRule(AutoConfigRule rule)
    {
        _rules.Add(rule);
    }

    public void RemoveRule(string ruleId)
    {
        _rules.RemoveAll(r => r.Id == ruleId);
    }

    public List<AutoConfigRule> GetAllRules()
    {
        return _rules.ToList();
    }

    public void EnableRule(string ruleId, bool enabled)
    {
        var rule = _rules.FirstOrDefault(r => r.Id == ruleId);
        if (rule != null)
        {
            rule.Enabled = enabled;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using LJokerL.Core.Models;

namespace LJokerL.Core.Services;

public sealed class HardwareRuleEvaluator
{
    public HardwareRecommendationResult Evaluate(HardwareSnapshot snapshot, HardwareRuleCatalog? catalog)
    {
        var result = new HardwareRecommendationResult();
        if (catalog == null)
        {
            result.Warnings.Add("Hardware rules not loaded");
            return result;
        }

        var requiresVrr = catalog.Rules.Any(rule => rule.When?.MonitorVrr != null);
        if (requiresVrr && snapshot.Monitor?.VrrSupported == null)
        {
            result.Warnings.Add("Monitor VRR state unknown; some recommendations may not apply");
        }

        foreach (var rule in catalog.Rules)
        {
            if (!Matches(rule, snapshot))
            {
                continue;
            }

            if (rule.Recommend == null)
            {
                continue;
            }

            result.Matches.Add(new HardwareRuleMatch(rule.Id, rule.Recommend));
            MergeRecommendation(result.Aggregate, rule.Recommend);
        }

        return result;
    }

    private static bool Matches(HardwareRuleDefinition rule, HardwareSnapshot snapshot)
    {
        if (rule.When == null)
        {
            return true;
        }

        // Monitor VRR check
        if (rule.When.MonitorVrr.HasValue)
        {
            var vrr = snapshot.Monitor?.VrrSupported;
            if (!vrr.HasValue || vrr.Value != rule.When.MonitorVrr.Value)
            {
                return false;
            }
        }

        // CPU checks
        if (rule.When.CpuCoresMin.HasValue)
        {
            var cores = snapshot.Cpu?.Cores;
            if (!cores.HasValue || cores.Value < rule.When.CpuCoresMin.Value)
            {
                return false;
            }
        }

        if (rule.When.CpuCoresMax.HasValue)
        {
            var cores = snapshot.Cpu?.Cores;
            if (!cores.HasValue || cores.Value > rule.When.CpuCoresMax.Value)
            {
                return false;
            }
        }

        if (!string.IsNullOrWhiteSpace(rule.When.CpuVendor))
        {
            var vendor = snapshot.Cpu?.Manufacturer;
            if (string.IsNullOrWhiteSpace(vendor) || 
                !vendor.Contains(rule.When.CpuVendor, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        // RAM checks
        if (rule.When.RamGbMin.HasValue)
        {
            var ramGb = snapshot.Memory?.TotalBytes / (1024.0 * 1024.0 * 1024.0);
            if (!ramGb.HasValue || ramGb.Value < rule.When.RamGbMin.Value)
            {
                return false;
            }
        }

        if (rule.When.RamGbMax.HasValue)
        {
            var ramGb = snapshot.Memory?.TotalBytes / (1024.0 * 1024.0 * 1024.0);
            if (!ramGb.HasValue || ramGb.Value > rule.When.RamGbMax.Value)
            {
                return false;
            }
        }

        // GPU checks
        if (!string.IsNullOrWhiteSpace(rule.When.GpuVendor))
        {
            var vendor = snapshot.Gpu?.Vendor;
            if (string.IsNullOrWhiteSpace(vendor) || 
                !vendor.Contains(rule.When.GpuVendor, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        if (rule.When.GpuVramMin.HasValue)
        {
            var vramMb = snapshot.Gpu?.VramBytes / (1024.0 * 1024.0);
            if (!vramMb.HasValue || vramMb.Value < rule.When.GpuVramMin.Value)
            {
                return false;
            }
        }

        if (rule.When.GpuVramMax.HasValue)
        {
            var vramMb = snapshot.Gpu?.VramBytes / (1024.0 * 1024.0);
            if (!vramMb.HasValue || vramMb.Value > rule.When.GpuVramMax.Value)
            {
                return false;
            }
        }

        return true;
    }

    private static void MergeRecommendation(HardwareRuleRecommendation aggregate, HardwareRuleRecommendation incoming)
    {
        if (incoming.FpsCapOffset.HasValue)
            aggregate.FpsCapOffset = incoming.FpsCapOffset;

        if (incoming.VsyncDriver.HasValue)
            aggregate.VsyncDriver = incoming.VsyncDriver;

        if (incoming.VsyncInGame.HasValue)
            aggregate.VsyncInGame = incoming.VsyncInGame;

        if (incoming.DisableBackgroundApps.HasValue)
            aggregate.DisableBackgroundApps = incoming.DisableBackgroundApps;

        if (incoming.PageFileSizeGb.HasValue)
            aggregate.PageFileSizeGb = incoming.PageFileSizeGb;

        if (!string.IsNullOrWhiteSpace(incoming.TextureQuality))
            aggregate.TextureQuality = incoming.TextureQuality;

        if (incoming.UsePCoresOnly.HasValue)
            aggregate.UsePCoresOnly = incoming.UsePCoresOnly;

        if (!string.IsNullOrWhiteSpace(incoming.ThreadDirectorMode))
            aggregate.ThreadDirectorMode = incoming.ThreadDirectorMode;

        if (!string.IsNullOrWhiteSpace(incoming.LowLatencyMode))
            aggregate.LowLatencyMode = incoming.LowLatencyMode;

        if (!string.IsNullOrWhiteSpace(incoming.PowerManagement))
            aggregate.PowerManagement = incoming.PowerManagement;

        if (incoming.EnableGameMode.HasValue)
            aggregate.EnableGameMode = incoming.EnableGameMode;

        if (incoming.DisableFullscreenOptimizations.HasValue)
            aggregate.DisableFullscreenOptimizations = incoming.DisableFullscreenOptimizations;

        if (incoming.EnableHpet.HasValue)
            aggregate.EnableHpet = incoming.EnableHpet;

        if (incoming.TimerResolution.HasValue)
            aggregate.TimerResolution = incoming.TimerResolution;
    }
}

public sealed class HardwareRecommendationResult
{
    public List<HardwareRuleMatch> Matches { get; } = new();
    public HardwareRuleRecommendation Aggregate { get; } = new();
    public List<string> Warnings { get; } = new();
}

public sealed class HardwareRuleMatch
{
    public HardwareRuleMatch(string ruleId, HardwareRuleRecommendation recommendation)
    {
        RuleId = ruleId;
        Recommendation = recommendation;
    }

    public string RuleId { get; }
    public HardwareRuleRecommendation Recommendation { get; }
}

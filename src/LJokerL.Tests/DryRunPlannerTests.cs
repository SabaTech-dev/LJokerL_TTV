using LJokerL.Core.Models;
using LJokerL.Core.Services;
using Xunit;

namespace LJokerL.Tests;

public class DryRunPlannerTests
{
    private static ConfigBundle LoadRealBundle()
    {
        var configRoot = Path.GetDirectoryName(TestHelpers.FindConfigPath("profiles.json"))!;
        return new ConfigLoader().LoadAll(configRoot);
    }

    private static ProfileDefinition FindProfile(ConfigBundle bundle, string id)
    {
        return bundle.Profiles!.Profiles
            .Single(p => string.Equals(p.Id, id, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Gaming_pro_plan_is_not_empty_and_contains_registry_service_and_app_kinds()
    {
        var bundle = LoadRealBundle();
        var profile = FindProfile(bundle, "gaming_pro");

        var plan = DryRunPlanner.Plan(
            profile,
            bundle.Tweaks?.Tweaks,
            bundle.Services?.Services,
            bundle.Apps?.Apps);

        Assert.NotEmpty(plan.Actions);
        Assert.Empty(plan.Skipped);
        Assert.True(plan.CountByKind(DryRunActionKind.Registry) > 0, "gaming_pro should plan registry actions");
        Assert.True(plan.CountByKind(DryRunActionKind.Service) > 0, "gaming_pro should plan service actions");
        Assert.True(plan.CountByKind(DryRunActionKind.App) > 0, "gaming_pro should plan app actions");
    }

    [Fact]
    public void Balanced_plan_is_not_empty_and_contains_registry_service_and_app_kinds()
    {
        var bundle = LoadRealBundle();
        var profile = FindProfile(bundle, "balanced");

        var plan = DryRunPlanner.Plan(
            profile,
            bundle.Tweaks?.Tweaks,
            bundle.Services?.Services,
            bundle.Apps?.Apps);

        Assert.NotEmpty(plan.Actions);
        Assert.Empty(plan.Skipped);
        Assert.True(plan.CountByKind(DryRunActionKind.Registry) > 0, "balanced should plan registry actions");
        Assert.True(plan.CountByKind(DryRunActionKind.Service) > 0, "balanced should plan service actions");
        Assert.True(plan.CountByKind(DryRunActionKind.App) > 0, "balanced should plan app actions");
    }

    [Fact]
    public void Plan_omits_unknown_ids_without_failing()
    {
        var bundle = LoadRealBundle();
        var profile = new ProfileDefinition
        {
            Id = "test_missing_ids",
            Name = "Test missing ids",
            Tweaks = new List<string> { "game_dvr_off", "no_such_tweak" },
            Services = new List<string> { "diagtrack", "no_such_service" },
            Apps = new List<string> { "cortana", "no_such_app" }
        };

        var plan = DryRunPlanner.Plan(
            profile,
            bundle.Tweaks?.Tweaks,
            bundle.Services?.Services,
            bundle.Apps?.Apps);

        Assert.Contains(plan.Actions, a => a.Kind == DryRunActionKind.Registry && a.Id == "game_dvr_off");
        Assert.Contains(plan.Actions, a => a.Kind == DryRunActionKind.Service && a.Id == "diagtrack");
        Assert.Contains(plan.Actions, a => a.Kind == DryRunActionKind.App && a.Id == "cortana");
        Assert.DoesNotContain(plan.Actions, a => a.Id == "no_such_tweak");
        Assert.DoesNotContain(plan.Actions, a => a.Id == "no_such_service");
        Assert.DoesNotContain(plan.Actions, a => a.Id == "no_such_app");
        Assert.Equal(3, plan.Skipped.Count);
        Assert.Contains(plan.Skipped, s => s.Contains("tweak:no_such_tweak", StringComparison.Ordinal));
        Assert.Contains(plan.Skipped, s => s.Contains("service:no_such_service", StringComparison.Ordinal));
        Assert.Contains(plan.Skipped, s => s.Contains("app:no_such_app", StringComparison.Ordinal));
    }

    [Fact]
    public void Planned_reversibility_reflects_catalog_revert_actions()
    {
        var bundle = LoadRealBundle();
        var tweakMap = (bundle.Tweaks?.Tweaks ?? new List<TweakDefinition>())
            .GroupBy(t => t.Id, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

        foreach (var profile in bundle.Profiles!.Profiles)
        {
            var plan = DryRunPlanner.Plan(
                profile,
                bundle.Tweaks?.Tweaks,
                bundle.Services?.Services,
                bundle.Apps?.Apps);

            foreach (var action in plan.Actions)
            {
                if (action.Kind is DryRunActionKind.Registry or DryRunActionKind.Command)
                {
                    var tweak = tweakMap[action.Id];
                    Assert.True(
                        action.Reversible == (tweak.Revert.Count > 0),
                        $"PlannedAction {action.Kind}:{action.Id} reversibility must match catalog revert actions");
                }
                else
                {
                    Assert.True(action.Reversible, $"{action.Kind}:{action.Id} should be reversible via engine revert");
                }
            }
        }
    }
}

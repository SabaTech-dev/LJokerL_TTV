using LJokerL.Core.Models;
using LJokerL.Core.Services;
using Xunit;

namespace LJokerL.Tests;

public class GameWatcherPlannerTests
{
    private static GameWatcherCatalog BuildCatalog(params GameWatcherMapping[] mappings)
    {
        return new GameWatcherCatalog
        {
            Version = "2026.10",
            Mappings = new List<GameWatcherMapping>(mappings)
        };
    }

    [Fact]
    public void Decide_MatchIsCaseInsensitiveAndIgnoresExeSuffix_AppliesMappedProfile()
    {
        var catalog = BuildCatalog(new GameWatcherMapping { ProcessName = "cyberpunk2077", ProfileId = "gaming_pro", Enabled = true });

        var decision = GameWatcherPlanner.Decide(new[] { "CYBERPUNK2077.EXE" }, catalog, currentlyAppliedProfileId: null);

        Assert.Equal(GameWatcherAction.Apply, decision.Action);
        Assert.Equal("gaming_pro", decision.ProfileId);
        Assert.Contains("cyberpunk2077", decision.Reason, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Decide_DisabledMappingIsIgnored()
    {
        var catalog = BuildCatalog(new GameWatcherMapping { ProcessName = "notepad", ProfileId = "balanced", Enabled = false });

        var decision = GameWatcherPlanner.Decide(new[] { "notepad.exe" }, catalog, currentlyAppliedProfileId: null);

        Assert.Equal(GameWatcherAction.None, decision.Action);
        Assert.Null(decision.ProfileId);
    }

    [Fact]
    public void Decide_ProfileAlreadyApplied_ReturnsNoneWithAlreadyAppliedReason()
    {
        var catalog = BuildCatalog(new GameWatcherMapping { ProcessName = "cyberpunk2077", ProfileId = "gaming_pro", Enabled = true });

        var decision = GameWatcherPlanner.Decide(new[] { "cyberpunk2077.exe" }, catalog, currentlyAppliedProfileId: "gaming_pro");

        Assert.Equal(GameWatcherAction.None, decision.Action);
        Assert.Null(decision.ProfileId);
        Assert.Contains("ya aplicado", decision.Reason, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Decide_NoRunningMatch_ReturnsNone()
    {
        var catalog = BuildCatalog(new GameWatcherMapping { ProcessName = "cyberpunk2077", ProfileId = "gaming_pro", Enabled = true });

        var decision = GameWatcherPlanner.Decide(new[] { "chrome.exe", "explorer.exe" }, catalog, currentlyAppliedProfileId: null);

        Assert.Equal(GameWatcherAction.None, decision.Action);
        Assert.Null(decision.ProfileId);
    }

    [Fact]
    public void Decide_ConflictingMatches_PicksFirstInCatalogAndAddsConflictNote()
    {
        var catalog = BuildCatalog(
            new GameWatcherMapping { ProcessName = "gameb", ProfileId = "balanced", Enabled = true },
            new GameWatcherMapping { ProcessName = "gamea", ProfileId = "gaming_pro", Enabled = true });

        var decision = GameWatcherPlanner.Decide(new[] { "gamea.exe", "GAMEB.EXE" }, catalog, currentlyAppliedProfileId: null);

        Assert.Equal(GameWatcherAction.Apply, decision.Action);
        Assert.Equal("balanced", decision.ProfileId);
        Assert.Contains("conflicto", decision.Reason, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("gamea", decision.Reason, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Decide_EmptyCatalog_ReturnsNone()
    {
        var catalog = BuildCatalog();

        var decision = GameWatcherPlanner.Decide(new[] { "cyberpunk2077.exe" }, catalog, currentlyAppliedProfileId: null);

        Assert.Equal(GameWatcherAction.None, decision.Action);
        Assert.Null(decision.ProfileId);
    }

    [Fact]
    public void Decide_AbortedApplyIsNotMarked_PlannerReoffersProfile()
    {
        var catalog = BuildCatalog(new GameWatcherMapping { ProcessName = "cyberpunk2077", ProfileId = "gaming_pro", Enabled = true });

        var firstDecision = GameWatcherPlanner.Decide(new[] { "cyberpunk2077.exe" }, catalog, currentlyAppliedProfileId: null);
        Assert.Equal(GameWatcherAction.Apply, firstDecision.Action);

        // Simulated aborted apply: the real applied-profile tracker stays null.
        var retryDecision = GameWatcherPlanner.Decide(new[] { "cyberpunk2077.exe" }, catalog, currentlyAppliedProfileId: null);
        Assert.Equal(GameWatcherAction.Apply, retryDecision.Action);
        Assert.Equal("gaming_pro", retryDecision.ProfileId);

        // Contrast: a successful apply marks the profile and the planner stops re-applying.
        var afterSuccess = GameWatcherPlanner.Decide(new[] { "cyberpunk2077.exe" }, catalog, currentlyAppliedProfileId: "gaming_pro");
        Assert.Equal(GameWatcherAction.None, afterSuccess.Action);
    }
}

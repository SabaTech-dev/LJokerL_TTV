using LJokerL.Core.Services;
using Xunit;

namespace LJokerL.Tests;

/// <summary>
/// Decision matrix for SystemRestoreGate (M1-A fail-safe):
/// - setting off -> Proceed (feature not requested).
/// - setting on + System Restore disabled -> SkipFailSafe (protection cannot be verified).
/// - setting on + restore point newer than 24h -> ProceedRecentPoint (Windows caps creation at 1/24h; do not retry).
/// - setting on + no recent point + creation succeeded -> Proceed.
/// - setting on + no recent point + creation failed -> SkipFailSafe (never apply without the snapshot).
/// </summary>
public class RestoreGateTests
{
    private static readonly DateTime Now = new(2026, 10, 27, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void SettingDisabled_proceeds_even_if_creation_failed()
    {
        var result = SystemRestoreGate.Decide(
            settingEnabled: false, systemRestoreEnabled: false, lastPointUtc: null, nowUtc: Now, creationSucceeded: false);

        Assert.Equal(SystemRestoreDecision.Proceed, result.Decision);
        Assert.Equal(SystemRestoreSkipReason.None, result.Reason);
    }

    [Fact]
    public void SettingOn_systemRestoreDisabled_skipsFailSafe()
    {
        var result = SystemRestoreGate.Decide(
            settingEnabled: true, systemRestoreEnabled: false, lastPointUtc: null, nowUtc: Now, creationSucceeded: false);

        Assert.Equal(SystemRestoreDecision.SkipFailSafe, result.Decision);
        Assert.Equal(SystemRestoreSkipReason.SystemRestoreDisabled, result.Reason);
    }

    [Fact]
    public void SettingOn_recentPoint23h_proceedsRecentPoint()
    {
        var result = SystemRestoreGate.Decide(
            settingEnabled: true, systemRestoreEnabled: true, lastPointUtc: Now.AddHours(-23), nowUtc: Now, creationSucceeded: false);

        Assert.Equal(SystemRestoreDecision.ProceedRecentPoint, result.Decision);
        Assert.Equal(SystemRestoreSkipReason.None, result.Reason);
    }

    [Fact]
    public void SettingOn_recentPoint_ignoresCreationResult()
    {
        var result = SystemRestoreGate.Decide(
            settingEnabled: true, systemRestoreEnabled: true, lastPointUtc: Now.AddHours(-1), nowUtc: Now, creationSucceeded: true);

        Assert.Equal(SystemRestoreDecision.ProceedRecentPoint, result.Decision);
    }

    [Fact]
    public void SettingOn_futurePointTimestamp_treatedAsRecent()
    {
        var result = SystemRestoreGate.Decide(
            settingEnabled: true, systemRestoreEnabled: true, lastPointUtc: Now.AddHours(2), nowUtc: Now, creationSucceeded: false);

        Assert.Equal(SystemRestoreDecision.ProceedRecentPoint, result.Decision);
    }

    [Fact]
    public void SettingOn_pointExactly24h_isNotRecent()
    {
        var result = SystemRestoreGate.Decide(
            settingEnabled: true, systemRestoreEnabled: true, lastPointUtc: Now.AddHours(-24), nowUtc: Now, creationSucceeded: true);

        Assert.Equal(SystemRestoreDecision.Proceed, result.Decision);
        Assert.Equal(SystemRestoreSkipReason.None, result.Reason);
    }

    [Fact]
    public void SettingOn_noPoint_creationOk_proceeds()
    {
        var result = SystemRestoreGate.Decide(
            settingEnabled: true, systemRestoreEnabled: true, lastPointUtc: null, nowUtc: Now, creationSucceeded: true);

        Assert.Equal(SystemRestoreDecision.Proceed, result.Decision);
        Assert.Equal(SystemRestoreSkipReason.None, result.Reason);
    }

    [Fact]
    public void SettingOn_oldPoint48h_creationOk_proceeds()
    {
        var result = SystemRestoreGate.Decide(
            settingEnabled: true, systemRestoreEnabled: true, lastPointUtc: Now.AddHours(-48), nowUtc: Now, creationSucceeded: true);

        Assert.Equal(SystemRestoreDecision.Proceed, result.Decision);
    }

    [Fact]
    public void SettingOn_noPoint_creationFailed_skipsFailSafe()
    {
        var result = SystemRestoreGate.Decide(
            settingEnabled: true, systemRestoreEnabled: true, lastPointUtc: null, nowUtc: Now, creationSucceeded: false);

        Assert.Equal(SystemRestoreDecision.SkipFailSafe, result.Decision);
        Assert.Equal(SystemRestoreSkipReason.CreationFailed, result.Reason);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
    }

    [Fact]
    public void SettingOn_oldPoint_creationFailed_skipsFailSafe()
    {
        var result = SystemRestoreGate.Decide(
            settingEnabled: true, systemRestoreEnabled: true, lastPointUtc: Now.AddHours(-48), nowUtc: Now, creationSucceeded: false);

        Assert.Equal(SystemRestoreDecision.SkipFailSafe, result.Decision);
        Assert.Equal(SystemRestoreSkipReason.CreationFailed, result.Reason);
    }
}

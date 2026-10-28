using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using LJokerL.Core.Models;

namespace LJokerL.Core.Services;

/// <summary>
/// Polls running processes at a fixed interval and, when the opt-in setting is
/// active, asks <see cref="GameWatcherPlanner"/> for a decision. The consumer
/// (view model) owns the apply path and must marshal it to the UI thread.
/// </summary>
public sealed class GameWatcherService : IDisposable
{
    public static readonly TimeSpan DefaultPollInterval = TimeSpan.FromSeconds(5);

    private readonly Func<bool> _isEnabled;
    private readonly Func<GameWatcherCatalog?> _catalogProvider;
    private readonly Func<string?> _currentlyAppliedProfileProvider;
    private readonly Action<GameWatcherDecision> _onDecision;
    private readonly TimeSpan _pollInterval;
    private Timer? _timer;
    private int _pollInProgress;

    public GameWatcherService(
        Func<bool> isEnabled,
        Func<GameWatcherCatalog?> catalogProvider,
        Func<string?> currentlyAppliedProfileProvider,
        Action<GameWatcherDecision> onDecision,
        TimeSpan? pollInterval = null)
    {
        _isEnabled = isEnabled ?? throw new ArgumentNullException(nameof(isEnabled));
        _catalogProvider = catalogProvider ?? throw new ArgumentNullException(nameof(catalogProvider));
        _currentlyAppliedProfileProvider = currentlyAppliedProfileProvider ?? throw new ArgumentNullException(nameof(currentlyAppliedProfileProvider));
        _onDecision = onDecision ?? throw new ArgumentNullException(nameof(onDecision));
        _pollInterval = pollInterval ?? DefaultPollInterval;
    }

    /// <summary>Reads the live opt-in setting.</summary>
    public bool IsEnabled() => _isEnabled();

    /// <summary>Snapshot of running process names: lowercase, without ".exe", distinct.</summary>
    public static IReadOnlyList<string> SnapshotRunningGameNames()
    {
        var names = new SortedSet<string>(StringComparer.Ordinal);
        foreach (var process in Process.GetProcesses())
        {
            var normalized = GameWatcherPlanner.Normalize(process.ProcessName);
            if (normalized.Length > 0)
            {
                names.Add(normalized);
            }

            process.Dispose();
        }

        return new List<string>(names);
    }

    public void Start()
    {
        if (_timer != null)
        {
            return;
        }

        _timer = new Timer(_ => Poll(), null, _pollInterval, _pollInterval);
    }

    public void Stop()
    {
        var timer = Interlocked.Exchange(ref _timer, null);
        timer?.Dispose();
    }

    public void Dispose() => Stop();

    /// <summary>
    /// One poll cycle: snapshots processes and runs the pure planner. Returns the
    /// decision; only an <see cref="GameWatcherAction.Apply"/> decision is forwarded
    /// to the consumer callback.
    /// </summary>
    public GameWatcherDecision PollOnce()
    {
        var catalog = _catalogProvider();
        var running = SnapshotRunningGameNames();
        var decision = GameWatcherPlanner.Decide(running.ToArray(), catalog, _currentlyAppliedProfileProvider());
        return decision;
    }

    private void Poll()
    {
        if (Interlocked.Exchange(ref _pollInProgress, 1) == 1)
        {
            return;
        }

        try
        {
            if (!_isEnabled())
            {
                return;
            }

            var decision = PollOnce();
            if (decision.Action == GameWatcherAction.Apply)
            {
                _onDecision(decision);
            }
        }
        catch
        {
            // Never let a poll cycle crash the timer thread.
        }
        finally
        {
            Interlocked.Exchange(ref _pollInProgress, 0);
        }
    }
}

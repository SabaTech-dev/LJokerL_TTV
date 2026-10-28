using System;
using System.Collections.Generic;
using System.Linq;
using LJokerL.Core.Models;

namespace LJokerL.Core.Services;

public enum GameWatcherAction
{
    None,
    Apply
}

public sealed class GameWatcherDecision
{
    public GameWatcherDecision(GameWatcherAction action, string? profileId, string reason)
    {
        Action = action;
        ProfileId = profileId;
        Reason = reason;
    }

    public GameWatcherAction Action { get; }
    public string? ProfileId { get; }
    public string Reason { get; }
}

public static class GameWatcherPlanner
{
    public static GameWatcherDecision Decide(string[]? runningProcessNames, GameWatcherCatalog? catalog, string? currentlyAppliedProfileId)
    {
        if (catalog == null || catalog.Mappings.Count == 0)
        {
            return new GameWatcherDecision(GameWatcherAction.None, null, "catálogo vacío o ausente");
        }

        if (runningProcessNames == null || runningProcessNames.Length == 0)
        {
            return new GameWatcherDecision(GameWatcherAction.None, null, "sin procesos en ejecución");
        }

        var running = new HashSet<string>(StringComparer.Ordinal);
        foreach (var name in runningProcessNames)
        {
            var normalized = Normalize(name);
            if (normalized.Length > 0)
            {
                running.Add(normalized);
            }
        }

        GameWatcherMapping? chosen = null;
        var conflicts = new List<string>();
        foreach (var mapping in catalog.Mappings)
        {
            if (mapping == null || !mapping.Enabled)
            {
                continue;
            }

            var key = Normalize(mapping.ProcessName);
            if (key.Length == 0 || !running.Contains(key))
            {
                continue;
            }

            if (chosen == null)
            {
                chosen = mapping;
            }
            else
            {
                conflicts.Add(mapping.ProcessName);
            }
        }

        if (chosen == null)
        {
            return new GameWatcherDecision(GameWatcherAction.None, null, "sin coincidencia de procesos vigilados");
        }

        var reason = $"proceso detectado '{chosen.ProcessName}'";
        if (conflicts.Count > 0)
        {
            reason += $"; conflicto con {string.Join(", ", conflicts)}: se aplica el primero del catálogo";
        }

        if (string.Equals(chosen.ProfileId, currentlyAppliedProfileId, StringComparison.OrdinalIgnoreCase))
        {
            return new GameWatcherDecision(GameWatcherAction.None, null, $"{reason}; ya aplicado");
        }

        return new GameWatcherDecision(GameWatcherAction.Apply, chosen.ProfileId, reason);
    }

    public static string Normalize(string? processName)
    {
        var trimmed = (processName ?? string.Empty).Trim().ToLowerInvariant();
        return trimmed.EndsWith(".exe", StringComparison.Ordinal)
            ? trimmed[..^4]
            : trimmed;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using LJokerL.Core.Models;

namespace LJokerL.Core.Services;

public enum DryRunActionKind
{
    Registry,
    Command,
    Service,
    App
}

public sealed class PlannedAction
{
    public PlannedAction(DryRunActionKind kind, string id, string target, string operation, string risk, bool reversible)
    {
        Kind = kind;
        Id = id;
        Target = target;
        Operation = operation;
        Risk = risk;
        Reversible = reversible;
    }

    public DryRunActionKind Kind { get; }
    public string Id { get; }
    public string Target { get; }
    public string Operation { get; }
    public string Risk { get; }
    public bool Reversible { get; }
}

public sealed class DryRunPlan
{
    public DryRunPlan(string profileId, IReadOnlyList<PlannedAction> actions, IReadOnlyList<string> skipped)
    {
        ProfileId = profileId;
        Actions = actions;
        Skipped = skipped;
    }

    public string ProfileId { get; }
    public IReadOnlyList<PlannedAction> Actions { get; }
    public IReadOnlyList<string> Skipped { get; }

    public int CountByKind(DryRunActionKind kind) => Actions.Count(a => a.Kind == kind);

    public int ReversibleCount => Actions.Count(a => a.Reversible);

    public string Summarize()
    {
        var parts = string.Join(", ", new[]
        {
            $"registry {CountByKind(DryRunActionKind.Registry)}",
            $"command {CountByKind(DryRunActionKind.Command)}",
            $"service {CountByKind(DryRunActionKind.Service)}",
            $"app {CountByKind(DryRunActionKind.App)}"
        });

        return $"{Actions.Count} acciones ({parts}); reversibles {ReversibleCount}; omitidos {Skipped.Count}";
    }
}

public static class DryRunPlanner
{
    public static DryRunPlan Plan(
        ProfileDefinition profile,
        IEnumerable<TweakDefinition>? tweaks,
        IEnumerable<ServiceDefinition>? services,
        IEnumerable<AppPackageDefinition>? apps)
    {
        ArgumentNullException.ThrowIfNull(profile);
        return Plan(profile.Id, profile.Tweaks, profile.Services, profile.Apps, tweaks, services, apps);
    }

    public static DryRunPlan Plan(
        string profileId,
        IReadOnlyList<string>? tweakIds,
        IReadOnlyList<string>? serviceIds,
        IReadOnlyList<string>? appIds,
        IEnumerable<TweakDefinition>? tweaks,
        IEnumerable<ServiceDefinition>? services,
        IEnumerable<AppPackageDefinition>? apps)
    {
        var tweakMap = MapById(tweaks, t => t.Id);
        var serviceMap = MapById(services, s => s.Id);
        var appMap = MapById(apps, a => a.Id);

        var actions = new List<PlannedAction>();
        var skipped = new List<string>();

        foreach (var tweakId in tweakIds ?? Array.Empty<string>())
        {
            if (string.IsNullOrWhiteSpace(tweakId))
            {
                continue;
            }

            if (!tweakMap.TryGetValue(tweakId, out var tweak))
            {
                skipped.Add($"tweak:{tweakId} (missing)");
                continue;
            }

            var reversible = tweak.Revert.Count > 0;
            foreach (var action in tweak.Actions)
            {
                var type = action.Type?.Trim().ToLowerInvariant();
                if (type == "registry")
                {
                    var target = string.IsNullOrWhiteSpace(action.Name)
                        ? action.Path ?? string.Empty
                        : $"{action.Path}\\{action.Name}";
                    var operation = string.Equals(action.Operation, "delete", StringComparison.OrdinalIgnoreCase)
                        ? "delete"
                        : "set";
                    actions.Add(new PlannedAction(DryRunActionKind.Registry, tweak.Id, target, operation, tweak.Risk, reversible));
                }
                else if (type == "command")
                {
                    actions.Add(new PlannedAction(DryRunActionKind.Command, tweak.Id, action.Command ?? string.Empty, "run", tweak.Risk, reversible));
                }
            }
        }

        foreach (var serviceId in serviceIds ?? Array.Empty<string>())
        {
            if (string.IsNullOrWhiteSpace(serviceId))
            {
                continue;
            }

            if (!serviceMap.TryGetValue(serviceId, out var service))
            {
                skipped.Add($"service:{serviceId} (missing)");
                continue;
            }

            actions.Add(new PlannedAction(DryRunActionKind.Service, service.Id, service.Name, "disable", service.Risk, true));
        }

        foreach (var appId in appIds ?? Array.Empty<string>())
        {
            if (string.IsNullOrWhiteSpace(appId))
            {
                continue;
            }

            if (!appMap.TryGetValue(appId, out var app))
            {
                skipped.Add($"app:{appId} (missing)");
                continue;
            }

            actions.Add(new PlannedAction(DryRunActionKind.App, app.Id, app.PackageName, "remove", app.Risk, true));
        }

        return new DryRunPlan(profileId, actions, skipped);
    }

    private static Dictionary<string, T> MapById<T>(IEnumerable<T>? items, Func<T, string> keySelector)
        where T : class
    {
        var map = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in items ?? Enumerable.Empty<T>())
        {
            var key = keySelector(item);
            if (!string.IsNullOrWhiteSpace(key) && !map.ContainsKey(key))
            {
                map.Add(key, item);
            }
        }

        return map;
    }
}

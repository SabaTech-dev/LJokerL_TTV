using System;
using System.Collections.Generic;
using LJokerL.Core.Models;

namespace LJokerL.Core.Services;

public static class BenchmarkComparer
{
    public static BenchmarkComparisonResult Compare(BenchmarkRunResult before, BenchmarkRunResult after)
    {
        var result = new BenchmarkComparisonResult
        {
            Deltas = new List<BenchmarkDelta>()
        };

        AddDelta(result, "CPU", before.Cpu?.Score, after.Cpu?.Score);
        AddDelta(result, "Memory", before.Memory?.Score, after.Memory?.Score);
        AddDelta(result, "Disk", before.Disk?.Score, after.Disk?.Score);
        AddDelta(result, "Network", before.Network?.Score, after.Network?.Score);
        AddDelta(result, "System", before.System?.Score, after.System?.Score);

        var overallBefore = before.OverallScore;
        var overallAfter = after.OverallScore;
        result.OverallBefore = overallBefore;
        result.OverallAfter = overallAfter;

        if (overallBefore.HasValue && overallAfter.HasValue)
        {
            var delta = overallAfter.Value - overallBefore.Value;
            result.OverallDelta = delta;
            result.OverallPercentChange = overallBefore.Value == 0
                ? null
                : Math.Round(delta / overallBefore.Value * 100, 2);
        }

        // CPU Jitter
        if (before.Cpu != null && after.Cpu != null)
        {
            result.CpuJitterBefore = before.Cpu.ThreadSchedulingJitterMs;
            result.CpuJitterAfter = after.Cpu.ThreadSchedulingJitterMs;
            result.CpuJitterDelta = Math.Round(after.Cpu.ThreadSchedulingJitterMs - before.Cpu.ThreadSchedulingJitterMs, 3);
            result.CpuJitterPercentChange = before.Cpu.ThreadSchedulingJitterMs == 0
                ? null
                : Math.Round((after.Cpu.ThreadSchedulingJitterMs - before.Cpu.ThreadSchedulingJitterMs) / before.Cpu.ThreadSchedulingJitterMs * 100, 2);
        }

        // Memory Latency
        if (before.Memory != null && after.Memory != null)
        {
            result.MemLatencyBefore = before.Memory.LatencyPointerChasingNs;
            result.MemLatencyAfter = after.Memory.LatencyPointerChasingNs;
            result.MemLatencyDelta = Math.Round(after.Memory.LatencyPointerChasingNs - before.Memory.LatencyPointerChasingNs, 2);
            result.MemLatencyPercentChange = before.Memory.LatencyPointerChasingNs == 0
                ? null
                : Math.Round((after.Memory.LatencyPointerChasingNs - before.Memory.LatencyPointerChasingNs) / before.Memory.LatencyPointerChasingNs * 100, 2);
        }

        // Network Jitter
        if (before.Network != null && after.Network != null)
        {
            result.NetJitterBefore = before.Network.OverallJitterMs;
            result.NetJitterAfter = after.Network.OverallJitterMs;
            result.NetJitterDelta = Math.Round(after.Network.OverallJitterMs - before.Network.OverallJitterMs, 2);
            result.NetJitterPercentChange = before.Network.OverallJitterMs == 0
                ? null
                : Math.Round((after.Network.OverallJitterMs - before.Network.OverallJitterMs) / before.Network.OverallJitterMs * 100, 2);
        }

        return result;
    }

    private static void AddDelta(BenchmarkComparisonResult result, string name, double? before, double? after)
    {
        if (!before.HasValue || !after.HasValue)
        {
            return;
        }

        var delta = after.Value - before.Value;
        result.Deltas.Add(new BenchmarkDelta
        {
            Name = name,
            BeforeScore = before,
            AfterScore = after,
            Delta = Math.Round(delta, 2),
            PercentChange = before.Value == 0 ? null : Math.Round(delta / before.Value * 100, 2)
        });
    }
}

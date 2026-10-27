using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LJokerL.Core.Models;

public sealed class BenchmarkRunResult
{
    public BenchmarkInfo BenchmarkInfo { get; set; } = new();

    [JsonPropertyName("CPU")]
    public CpuBenchmarkResult? Cpu { get; set; }

    [JsonPropertyName("Memory")]
    public MemoryBenchmarkResult? Memory { get; set; }

    [JsonPropertyName("Disk")]
    public DiskBenchmarkResult? Disk { get; set; }

    [JsonPropertyName("Network")]
    public NetworkBenchmarkResult? Network { get; set; }

    [JsonPropertyName("System")]
    public SystemBenchmarkResult? System { get; set; }

    public double? OverallScore { get; set; }
}

public sealed class BenchmarkInfo
{
    public string Version { get; set; } = "";
    public string TestType { get; set; } = "";
    public string StartTime { get; set; } = "";
    public string EndTime { get; set; } = "";
    public double Duration { get; set; }
    public int TestsExecuted { get; set; }
}

public sealed class CpuBenchmarkResult
{
    public string TestName { get; set; } = "CPU Performance";
    public string? CPUName { get; set; }
    public int? Cores { get; set; }
    public int? Threads { get; set; }
    public int? BaseFrequency { get; set; }
    public int PrimesCalculated { get; set; }
    public double TestDuration { get; set; }
    public double PrimesPerSecond { get; set; }
    public double AverageCPUUsage { get; set; }
    public double ThreadSchedulingJitterMs { get; set; }
    public string? Temperature { get; set; }
    public double Score { get; set; }
    public string Timestamp { get; set; } = "";
}

public sealed class MemoryBenchmarkResult
{
    public string TestName { get; set; } = "Memory Performance";
    public double TotalRAM { get; set; }
    public double AvailableRAM { get; set; }
    public int ArraySize { get; set; }
    public double ArraySizeMB { get; set; }
    public double WriteTime { get; set; }
    public double ReadTime { get; set; }
    public double RandomAccessTime { get; set; }
    public double LatencyTime { get; set; }
    public double WriteSpeedMB { get; set; }
    public double ReadSpeedMB { get; set; }
    public double RandomSpeedK { get; set; }
    public double AvgLatencyMs { get; set; }
    public double LatencyPointerChasingNs { get; set; }
    public double WriteScore { get; set; }
    public double ReadScore { get; set; }
    public double RandomScore { get; set; }
    public double LatencyScore { get; set; }
    public double Score { get; set; }
    public string Timestamp { get; set; } = "";
}

public sealed class DiskBenchmarkResult
{
    public string TestName { get; set; } = "Disk Performance";
    public string? DriveModel { get; set; }
    public double DriveSize { get; set; }
    public double FreeSpace { get; set; }
    public double FileSize { get; set; }
    public double WriteTime { get; set; }
    public double ReadTime { get; set; }
    public double WriteSpeed { get; set; }
    public double ReadSpeed { get; set; }
    public double Score { get; set; }
    public string Timestamp { get; set; } = "";
}

public sealed class NetworkBenchmarkResult
{
    public string TestName { get; set; } = "Network Performance";
    public int NetworkAdapters { get; set; }
    public List<NetworkPingResult> PingResults { get; set; } = new();
    public double OverallLatency { get; set; }
    public double OverallJitterMs { get; set; }
    public double Score { get; set; }
    public string Timestamp { get; set; } = "";
}

public sealed class NetworkPingResult
{
    public string Target { get; set; } = "";
    public double AvgLatency { get; set; }
    public double MinLatency { get; set; }
    public double MaxLatency { get; set; }
    public double PacketLoss { get; set; }
    public double JitterMs { get; set; }
}

public sealed class SystemBenchmarkResult
{
    public string TestName { get; set; } = "System Performance";
    public string OSName { get; set; } = "";
    public string OSVersion { get; set; } = "";
    public string? ComputerModel { get; set; }
    public string? BIOSVersion { get; set; }
    public string LastBootTime { get; set; } = "";
    public string Uptime { get; set; } = "";
    public int RunningServices { get; set; }
    public int RunningProcesses { get; set; }
    public double CurrentCPUUsage { get; set; }
    public double CurrentMemoryUsage { get; set; }
    public double Score { get; set; }
    public string Timestamp { get; set; } = "";
}

public sealed class BenchmarkRunInfo
{
    public string FilePath { get; set; } = "";
    public string FileName { get; set; } = "";
    public string TestType { get; set; } = "";
    public string StartTime { get; set; } = "";
    public string EndTime { get; set; } = "";
    public double DurationSeconds { get; set; }
    public double? OverallScore { get; set; }
}

public sealed class BenchmarkProgress
{
    public string Stage { get; set; } = "";
    public int Completed { get; set; }
    public int Total { get; set; }
}

public sealed class BenchmarkDelta
{
    public string Name { get; set; } = "";
    public double? BeforeScore { get; set; }
    public double? AfterScore { get; set; }
    public double? Delta { get; set; }
    public double? PercentChange { get; set; }
}

public sealed class BenchmarkComparisonResult
{
    public List<BenchmarkDelta> Deltas { get; set; } = new();
    public double? OverallBefore { get; set; }
    public double? OverallAfter { get; set; }
    public double? OverallDelta { get; set; }
    public double? OverallPercentChange { get; set; }

    // Detailed Gaming Metrics
    public double? CpuJitterBefore { get; set; }
    public double? CpuJitterAfter { get; set; }
    public double? CpuJitterDelta { get; set; }
    public double? CpuJitterPercentChange { get; set; }

    public double? MemLatencyBefore { get; set; }
    public double? MemLatencyAfter { get; set; }
    public double? MemLatencyDelta { get; set; }
    public double? MemLatencyPercentChange { get; set; }

    public double? NetJitterBefore { get; set; }
    public double? NetJitterAfter { get; set; }
    public double? NetJitterDelta { get; set; }
    public double? NetJitterPercentChange { get; set; }
}

public sealed class BenchmarkRunOptions
{
    public string TestType { get; set; } = "Complete";
    public bool RunCpu { get; set; } = true;
    public bool RunMemory { get; set; } = true;
    public bool RunDisk { get; set; } = true;
    public bool RunNetwork { get; set; } = true;
    public bool RunSystem { get; set; } = true;
    public List<string> PingTargets { get; set; } = new() { "8.8.8.8", "1.1.1.1", "208.67.222.222" };
    public int PingCount { get; set; } = 10;
    public int CpuPrimeLimit { get; set; } = 50000;
    public int DiskFileSizeMb { get; set; } = 100;
    public int DiskBlockSizeKb { get; set; } = 1024;
    public int MemoryMaxMb { get; set; } = 200;

    public static BenchmarkRunOptions CreateQuick()
    {
        return new BenchmarkRunOptions
        {
            TestType = "Quick",
            RunDisk = false,
            RunNetwork = false
        };
    }
}

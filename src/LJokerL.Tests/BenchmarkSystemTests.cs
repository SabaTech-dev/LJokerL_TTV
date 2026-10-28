using LJokerL.Core.Models;
using LJokerL.Core.Services;
using Xunit;

namespace LJokerL.Tests;

public class BenchmarkSystemTests
{
    [Fact]
    public void BenchmarkRunner_measures_CPU_Jitter_and_memory_pointer_chasing_latency()
    {
        var runner = new BenchmarkRunner();
        // Run a quick benchmark (no network/disk to keep test execution extremely fast)
        var options = BenchmarkRunOptions.CreateQuick();
        options.RunCpu = true;
        options.RunMemory = true;
        options.CpuPrimeLimit = 1000; // Small prime limit for fast tests
        options.MemoryMaxMb = 10;     // Small memory footprint

        var result = runner.Run(options);

        Assert.True(result != null, "Benchmark failed: result is null.");
        Assert.True(result.Cpu != null, "Benchmark failed: CPU result is null.");
        Assert.True(result.Memory != null, "Benchmark failed: Memory result is null.");
        Assert.True(result.Cpu.ThreadSchedulingJitterMs > 0, $"Expected positive ThreadSchedulingJitterMs but was {result.Cpu.ThreadSchedulingJitterMs}.");
        Assert.True(result.Memory.LatencyPointerChasingNs > 0, $"Expected positive LatencyPointerChasingNs but was {result.Memory.LatencyPointerChasingNs}.");
    }

    [Fact]
    public void BenchmarkComparer_compares_runs_and_calculates_latency_deltas()
    {
        var before = new BenchmarkRunResult
        {
            OverallScore = 80.0,
            Cpu = new CpuBenchmarkResult { Score = 80, ThreadSchedulingJitterMs = 1.25 },
            Memory = new MemoryBenchmarkResult { Score = 85, LatencyPointerChasingNs = 55.4 },
            Network = new NetworkBenchmarkResult { Score = 90, OverallJitterMs = 4.2 }
        };

        var after = new BenchmarkRunResult
        {
            OverallScore = 90.0,
            Cpu = new CpuBenchmarkResult { Score = 92, ThreadSchedulingJitterMs = 0.85 },
            Memory = new MemoryBenchmarkResult { Score = 95, LatencyPointerChasingNs = 48.2 },
            Network = new NetworkBenchmarkResult { Score = 98, OverallJitterMs = 1.8 }
        };

        var comparison = BenchmarkComparer.Compare(before, after);

        Assert.True(comparison.OverallPercentChange.HasValue, "OverallPercentChange was not calculated.");
        Assert.Equal(Math.Round(0.85 - 1.25, 3), comparison.CpuJitterDelta);
        Assert.Equal(Math.Round(48.2 - 55.4, 2), comparison.MemLatencyDelta);
        Assert.Equal(Math.Round(1.8 - 4.2, 2), comparison.NetJitterDelta);
    }

    [Fact]
    public void BenchmarkReportGenerator_outputs_HTML_and_Markdown_reports_successfully()
    {
        var before = new BenchmarkRunResult
        {
            OverallScore = 80.0,
            Cpu = new CpuBenchmarkResult { Score = 80, ThreadSchedulingJitterMs = 1.25, CPUName = "Intel Test Core" },
            Memory = new MemoryBenchmarkResult { Score = 85, LatencyPointerChasingNs = 55.4, TotalRAM = 16.0 },
            Network = new NetworkBenchmarkResult { Score = 90, OverallJitterMs = 4.2 },
            System = new SystemBenchmarkResult { OSName = "Windows 11 Test", OSVersion = "22H2", ComputerModel = "Test PC", BIOSVersion = "V1.0" }
        };

        var after = new BenchmarkRunResult
        {
            OverallScore = 90.0,
            Cpu = new CpuBenchmarkResult { Score = 92, ThreadSchedulingJitterMs = 0.85, CPUName = "Intel Test Core" },
            Memory = new MemoryBenchmarkResult { Score = 95, LatencyPointerChasingNs = 48.2, TotalRAM = 16.0 },
            Network = new NetworkBenchmarkResult { Score = 98, OverallJitterMs = 1.8 },
            System = new SystemBenchmarkResult { OSName = "Windows 11 Test", OSVersion = "22H2", ComputerModel = "Test PC", BIOSVersion = "V1.0" }
        };

        var tempDir = Path.Combine(AppContext.BaseDirectory, "Benchmarks", "TestReports");
        var (htmlPath, mdPath) = BenchmarkReportGenerator.GenerateReport(before, after, tempDir);

        Assert.True(File.Exists(htmlPath), $"HTML report was not generated at '{htmlPath}'.");
        Assert.True(File.Exists(mdPath), $"Markdown report was not generated at '{mdPath}'.");

        var htmlContent = File.ReadAllText(htmlPath);
        var mdContent = File.ReadAllText(mdPath);

        Assert.True(htmlContent.Contains("Intel Test Core"), "HTML report does not contain the CPU name.");
        Assert.True(htmlContent.Contains("55.40ns"), "HTML report does not contain the expected memory latency.");
        Assert.True(mdContent.Contains("Intel Test Core"), "Markdown report does not contain the CPU name.");
        Assert.True(mdContent.Contains("55.40 ns"), "Markdown report does not contain the expected memory latency.");

        // Clean up files
        try
        {
            File.Delete(htmlPath);
            File.Delete(mdPath);
            Directory.Delete(tempDir);
        }
        catch { }
    }
}

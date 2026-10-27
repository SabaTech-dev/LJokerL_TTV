using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using LJokerL.Core.Models;

namespace LJokerL.Core.Services;

public sealed class BenchmarkStorage
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public IReadOnlyList<BenchmarkRunInfo> LoadRuns(string resultsPath)
    {
        var runs = new List<BenchmarkRunInfo>();
        if (!Directory.Exists(resultsPath))
        {
            return runs;
        }

        foreach (var file in Directory.GetFiles(resultsPath, "*.json").OrderByDescending(File.GetLastWriteTime))
        {
            var result = LoadResult(file);
            if (result == null)
            {
                continue;
            }

            var info = result.BenchmarkInfo ?? new BenchmarkInfo();
            var overallScore = result.OverallScore ?? ComputeOverallScore(result);

            runs.Add(new BenchmarkRunInfo
            {
                FilePath = file,
                FileName = Path.GetFileName(file),
                TestType = info.TestType,
                StartTime = info.StartTime,
                EndTime = info.EndTime,
                DurationSeconds = info.Duration,
                OverallScore = overallScore
            });
        }

        return runs;
    }

    public BenchmarkRunResult? LoadResult(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            var json = File.ReadAllText(filePath);
            var result = JsonSerializer.Deserialize<BenchmarkRunResult>(json, _options);
            if (result != null && result.OverallScore == null)
            {
                result.OverallScore = ComputeOverallScore(result);
            }

            return result;
        }
        catch
        {
            return null;
        }
    }

    public string SaveResult(BenchmarkRunResult result, string resultsPath)
    {
        Directory.CreateDirectory(resultsPath);
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var testType = string.IsNullOrWhiteSpace(result.BenchmarkInfo.TestType) ? "Complete" : result.BenchmarkInfo.TestType;
        var fileName = $"Benchmark_{testType}_{timestamp}.json";
        var filePath = Path.Combine(resultsPath, fileName);

        if (result.OverallScore == null)
        {
            result.OverallScore = ComputeOverallScore(result);
        }

        var json = JsonSerializer.Serialize(result, _options);
        File.WriteAllText(filePath, json);
        return filePath;
    }

    private static double ComputeOverallScore(BenchmarkRunResult result)
    {
        var scores = new List<double>();
        if (result.Cpu != null) scores.Add(result.Cpu.Score);
        if (result.Memory != null) scores.Add(result.Memory.Score);
        if (result.Disk != null) scores.Add(result.Disk.Score);
        if (result.Network != null) scores.Add(result.Network.Score);
        if (result.System != null) scores.Add(result.System.Score);

        return scores.Count == 0 ? 0 : Math.Round(scores.Average(), 2);
    }
}

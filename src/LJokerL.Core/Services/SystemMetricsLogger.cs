using System;
using System.IO;
using System.Text.Json;
using LJokerL.Core.Models;

namespace LJokerL.Core.Services;

public sealed class SystemMetricsLogger
{
    private readonly string _logPath;

    public SystemMetricsLogger(string logPath)
    {
        _logPath = string.IsNullOrWhiteSpace(logPath)
            ? Path.Combine(AppContext.BaseDirectory, "Logs", "metrics.jsonl")
            : logPath;
    }

    public string LogPath => _logPath;

    public void Log(SystemMetricsSnapshot snapshot)
    {
        try
        {
            var directory = Path.GetDirectoryName(_logPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(snapshot);
            File.AppendAllText(_logPath, json + Environment.NewLine);
        }
        catch
        {
        }
    }
}

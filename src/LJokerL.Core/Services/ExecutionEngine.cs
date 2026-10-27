using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.Json;
using System.Text.RegularExpressions;
using LJokerL.Core.Models;
using Microsoft.Win32;

namespace LJokerL.Core.Services;

public sealed class ExecutionEngine
{
    private readonly ExecutionLogger _logger;

    public ExecutionEngine(ExecutionLogger logger)
    {
        _logger = logger;
    }

    public ExecutionReport ApplyTweak(TweakDefinition tweak, string? contextId = null)
    {
        return ExecuteActions(contextId ?? tweak.Id, "apply", tweak.Actions);
    }

    public ExecutionReport RevertTweak(TweakDefinition tweak, string? contextId = null)
    {
        return ExecuteActions(contextId ?? tweak.Id, "revert", tweak.Revert);
    }

    public ExecutionReport ApplyService(ServiceDefinition service, string? contextId = null)
    {
        return ExecuteServiceAction(contextId ?? service.Id, "disable", service.Name);
    }

    public ExecutionReport RevertService(ServiceDefinition service, string? contextId = null)
    {
        return ExecuteServiceAction(contextId ?? service.Id, "enable", service.Name);
    }

    public ExecutionReport RemoveApp(AppPackageDefinition app, string? contextId = null)
    {
        var report = new ExecutionReport(contextId ?? app.Id, "remove_app");
        if (!IsSafePackageName(app.PackageName))
        {
            report.Actions.Add(ExecutionActionResult.Failed("command", "", "Invalid package name"));
            return report;
        }

        var safeName = app.PackageName.Trim();
        var shell = GetPowerShellCommand();
        var command = $"{shell} -NoProfile -ExecutionPolicy Bypass -Command \"Get-AppxPackage -Name '{safeName}' | Remove-AppxPackage\"";
        report.Actions.Add(ExecuteCommandRaw(contextId ?? app.Id, "remove_app", "command", command));
        return report;
    }

    public ExecutionReport RestoreApp(AppPackageDefinition app, string? contextId = null)
    {
        var report = new ExecutionReport(contextId ?? app.Id, "restore_app");
        if (!IsSafePackageName(app.PackageName))
        {
            report.Actions.Add(ExecutionActionResult.Failed("command", "", "Invalid package name"));
            return report;
        }

        var safeName = app.PackageName.Trim();
        var shell = GetPowerShellCommand();
        var command = $"{shell} -NoProfile -ExecutionPolicy Bypass -Command \"" +
                      $"Get-AppxPackage -AllUsers -Name '{safeName}' | ForEach-Object {{ " +
                      "Add-AppxPackage -DisableDevelopmentMode -Register \"$($_.InstallLocation)\\AppXManifest.xml\" }}\"";
        report.Actions.Add(ExecuteCommandRaw(contextId ?? app.Id, "restore_app", "command", command));
        return report;
    }

    public RollbackReport RollbackContext(string contextId)
    {
        var report = new RollbackReport(contextId);
        var entries = _logger.ReadEntries()
            .Where(entry => entry.ContextId == contextId && entry.Success)
            .ToList();

        entries.Reverse();

        foreach (var entry in entries)
        {
            if (string.Equals(entry.ActionType, "registry", StringComparison.OrdinalIgnoreCase))
            {
                var result = RollbackRegistry(entry);
                report.Actions.Add(result);
            }
            else
            {
                report.Actions.Add(RollbackActionResult.SkippedAction(entry.ActionType, entry.Target, "Rollback not supported for this action type"));
            }
        }

        return report;
    }

    public string? FindLatestContextForProfile(string profileId)
    {
        var prefix = $"profile:{profileId}:";
        string? latestContext = null;
        DateTime latestTimestamp = DateTime.MinValue;

        foreach (var entry in _logger.ReadEntries())
        {
            if (string.IsNullOrWhiteSpace(entry.ContextId))
            {
                continue;
            }

            if (!entry.ContextId.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (entry.TimestampUtc > latestTimestamp)
            {
                latestTimestamp = entry.TimestampUtc;
                latestContext = entry.ContextId;
            }
        }

        return latestContext;
    }

    private ExecutionReport ExecuteActions(string contextId, string mode, List<ActionDefinition> actions)
    {
        var report = new ExecutionReport(contextId, mode);
        foreach (var action in actions)
        {
            ExecutionActionResult result;
            switch (action.Type.Trim().ToLowerInvariant())
            {
                case "registry":
                    result = ExecuteRegistry(action, contextId, mode);
                    break;
                case "command":
                    result = ExecuteCommand(action, contextId, mode);
                    break;
                default:
                    result = ExecutionActionResult.Failed(action.Type, "", "Unsupported action type");
                    _logger.Log(new ExecutionLogEntry
                    {
                        TimestampUtc = DateTime.UtcNow,
                        ContextId = contextId,
                        Mode = mode,
                        ActionType = action.Type,
                        Target = action.Path ?? action.Command ?? "",
                        Success = false,
                        Error = "Unsupported action type"
                    });
                    break;
            }

            report.Actions.Add(result);
        }

        return report;
    }

    private ExecutionReport ExecuteServiceAction(string contextId, string mode, string serviceName)
    {
        var report = new ExecutionReport(contextId, mode);
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            report.Actions.Add(ExecutionActionResult.Failed("service", "", "Service name missing"));
            return report;
        }

        if (mode == "disable")
        {
            report.Actions.Add(ExecuteCommandRaw(contextId, mode, "service", $"sc stop \"{serviceName}\""));
            report.Actions.Add(ExecuteCommandRaw(contextId, mode, "service", $"sc config \"{serviceName}\" start= disabled"));
        }
        else
        {
            report.Actions.Add(ExecuteCommandRaw(contextId, mode, "service", $"sc config \"{serviceName}\" start= demand"));
            report.Actions.Add(ExecuteCommandRaw(contextId, mode, "service", $"sc start \"{serviceName}\""));
        }

        return report;
    }

    private ExecutionReport ExecuteCommandAction(string contextId, string mode, string command)
    {
        var report = new ExecutionReport(contextId, mode);
        report.Actions.Add(ExecuteCommandRaw(contextId, mode, "command", command));
        return report;
    }

    private ExecutionActionResult ExecuteRegistry(ActionDefinition action, string contextId, string mode)
    {
        if (string.IsNullOrWhiteSpace(action.Path) || string.IsNullOrWhiteSpace(action.Name))
        {
            return ExecutionActionResult.Failed("registry", "", "Registry path or name missing");
        }

        try
        {
            var (root, subKeyPath) = SplitRegistryPath(action.Path);
            using var key = root.CreateSubKey(subKeyPath, true);
            if (key == null)
            {
                return ExecutionActionResult.Failed("registry", action.Path, "Failed to open registry key");
            }

            var snapshot = CaptureRegistrySnapshot(key, action.Name);

            if (string.Equals(action.Operation, "delete", StringComparison.OrdinalIgnoreCase))
            {
                key.DeleteValue(action.Name, false);
                var deleteResult = ExecutionActionResult.Succeeded("registry", $"{action.Path}\\{action.Name}");
                _logger.Log(BuildLogEntry(contextId, mode, action, deleteResult, snapshot));
                return deleteResult;
            }

            var value = ParseRegistryValue(action.Value, action.ValueType);
            var kind = GetRegistryValueKind(action.ValueType);
            key.SetValue(action.Name, value ?? string.Empty, kind);

            var result = ExecutionActionResult.Succeeded("registry", $"{action.Path}\\{action.Name}");
            _logger.Log(BuildLogEntry(contextId, mode, action, result, snapshot));
            return result;
        }
        catch (UnauthorizedAccessException ex)
        {
            var result = ExecutionActionResult.Failed("registry", action.Path ?? "", "Access denied. Run as administrator.");
            _logger.Log(BuildLogEntry(contextId, mode, action, result, null, ex.ToString()));
            return result;
        }
        catch (SecurityException ex)
        {
            var result = ExecutionActionResult.Failed("registry", action.Path ?? "", "Security error accessing registry.");
            _logger.Log(BuildLogEntry(contextId, mode, action, result, null, ex.ToString()));
            return result;
        }
        catch (Exception ex)
        {
            var result = ExecutionActionResult.Failed("registry", action.Path ?? "", ex.Message);
            _logger.Log(BuildLogEntry(contextId, mode, action, result, null, ex.ToString()));
            return result;
        }
    }

    private ExecutionActionResult ExecuteCommand(ActionDefinition action, string contextId, string mode)
    {
        if (string.IsNullOrWhiteSpace(action.Command))
        {
            return ExecutionActionResult.Failed("command", "", "Command missing");
        }

        return ExecuteCommandRaw(contextId, mode, "command", action.Command, action);
    }

    private ExecutionActionResult ExecuteCommandRaw(string contextId, string mode, string actionType, string command, ActionDefinition? action = null)
    {
        var entry = new ExecutionLogEntry
        {
            TimestampUtc = DateTime.UtcNow,
            ContextId = contextId,
            Mode = mode,
            ActionType = actionType,
            Target = command
        };

        try
        {
            var cmdPath = GetCommandInterpreterPath();
            var psi = new ProcessStartInfo(cmdPath, $"/c {command}")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            if (process == null)
            {
                entry.Success = false;
                entry.Error = "Failed to start process";
                _logger.Log(entry);
                return ExecutionActionResult.Failed(actionType, command, "Failed to start process");
            }

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            entry.ExitCode = process.ExitCode;
            entry.Success = process.ExitCode == 0;
            if (!entry.Success)
            {
                entry.Error = string.IsNullOrWhiteSpace(error) ? output : error;
            }

            _logger.Log(entry);
            return entry.Success
                ? ExecutionActionResult.Succeeded(actionType, command, entry.ExitCode)
                : ExecutionActionResult.Failed(actionType, command, entry.Error ?? "Command failed", entry.ExitCode);
        }
        catch (Exception ex)
        {
            entry.Success = false;
            entry.Error = ex.Message;
            entry.ErrorDetails = ex.ToString();
            _logger.Log(entry);
            return ExecutionActionResult.Failed(actionType, command, ex.Message);
        }
    }

    private static (RegistryKey Root, string SubKeyPath) SplitRegistryPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new InvalidOperationException("Registry path is empty");
        }

        var parts = path.Trim().Split('\\', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            throw new InvalidOperationException($"Invalid registry path: {path}");
        }

        var root = parts[0].ToUpperInvariant();
        var subKeyPath = parts.Length > 1 ? parts[1] : "";
        return root switch
        {
            "HKLM" or "HKEY_LOCAL_MACHINE" => (Registry.LocalMachine, subKeyPath),
            "HKCU" or "HKEY_CURRENT_USER" => (Registry.CurrentUser, subKeyPath),
            "HKCR" or "HKEY_CLASSES_ROOT" => (Registry.ClassesRoot, subKeyPath),
            "HKU" or "HKEY_USERS" => (Registry.Users, subKeyPath),
            "HKCC" or "HKEY_CURRENT_CONFIG" => (Registry.CurrentConfig, subKeyPath),
            _ => throw new InvalidOperationException($"Unsupported registry root: {root} in {path}")
        };
    }

    private static RegistryValueKind GetRegistryValueKind(string? valueType)
    {
        return valueType?.Trim().ToUpperInvariant() switch
        {
            "DWORD" => RegistryValueKind.DWord,
            "QWORD" => RegistryValueKind.QWord,
            "EXPAND_STRING" => RegistryValueKind.ExpandString,
            "MULTI_STRING" => RegistryValueKind.MultiString,
            "BINARY" => RegistryValueKind.Binary,
            _ => RegistryValueKind.String
        };
    }

    private static RegistryValueKind ParseRegistryValueKind(string? valueType)
    {
        return valueType?.Trim().ToUpperInvariant() switch
        {
            "DWORD" => RegistryValueKind.DWord,
            "QWORD" => RegistryValueKind.QWord,
            "EXPAND_STRING" => RegistryValueKind.ExpandString,
            "MULTI_STRING" => RegistryValueKind.MultiString,
            "BINARY" => RegistryValueKind.Binary,
            _ => RegistryValueKind.String
        };
    }

    private static object? ParseLogValue(string? value, string? valueType)
    {
        if (value == null)
        {
            return null;
        }

        var kind = valueType?.Trim().ToUpperInvariant() ?? "STRING";
        return kind switch
        {
            "DWORD" => int.TryParse(value, out var dword) ? dword : 0,
            "QWORD" => long.TryParse(value, out var qword) ? qword : 0L,
            "MULTI_STRING" => JsonSerializer.Deserialize<string[]>(value) ?? Array.Empty<string>(),
            "BINARY" => Convert.FromBase64String(value),
            _ => value
        };
    }

    private static object? ParseRegistryValue(JsonElement? value, string? valueType)
    {
        if (value == null)
        {
            return null;
        }

        var kind = valueType?.Trim().ToUpperInvariant() ?? "STRING";
        return kind switch
        {
            "DWORD" => value.Value.ValueKind == JsonValueKind.Number
                ? value.Value.GetInt32()
                : int.TryParse(value.Value.ToString(), out var dword) ? dword : 0,
            "QWORD" => value.Value.ValueKind == JsonValueKind.Number
                ? value.Value.GetInt64()
                : long.TryParse(value.Value.ToString(), out var qword) ? qword : 0L,
            "MULTI_STRING" => value.Value.ValueKind == JsonValueKind.Array
                ? value.Value.EnumerateArray().Select(e => e.GetString() ?? string.Empty).ToArray()
                : new[] { value.Value.ToString() },
            "BINARY" => value.Value.ValueKind == JsonValueKind.String
                ? Convert.FromBase64String(value.Value.GetString() ?? string.Empty)
                : Array.Empty<byte>(),
            _ => value.Value.ValueKind == JsonValueKind.String ? value.Value.GetString() ?? string.Empty : value.Value.ToString()
        };
    }

    private static ExecutionLogEntry BuildLogEntry(string contextId, string mode, ActionDefinition action, ExecutionActionResult result, RegistrySnapshot? snapshot, string? errorDetails = null)
    {
        return new ExecutionLogEntry
        {
            TimestampUtc = DateTime.UtcNow,
            ContextId = contextId,
            Mode = mode,
            ActionType = action.Type,
            Target = action.Path ?? action.Command ?? string.Empty,
            RegistryName = action.Name,
            Operation = action.Operation,
            ValueType = action.ValueType,
            Value = action.Value?.ToString(),
            PreviousValue = snapshot?.Value,
            PreviousValueType = snapshot?.ValueType,
            Success = result.Success,
            Error = result.Error,
            ErrorDetails = errorDetails,
            ExitCode = result.ExitCode
        };
    }

    private static RegistrySnapshot? CaptureRegistrySnapshot(RegistryKey key, string valueName)
    {
        try
        {
            var rawValue = key.GetValue(valueName, null, RegistryValueOptions.DoNotExpandEnvironmentNames);
            if (rawValue == null)
            {
                return null;
            }

            RegistryValueKind? kind = null;
            try
            {
                kind = key.GetValueKind(valueName);
            }
            catch
            {
                kind = null;
            }

            var value = SerializeRegistryValue(rawValue, kind);
            return new RegistrySnapshot(value, kind?.ToString());
        }
        catch
        {
            return null;
        }
    }

    private static string? SerializeRegistryValue(object value, RegistryValueKind? kind)
    {
        if (kind == null)
        {
            return value.ToString();
        }

        return kind.Value switch
        {
            RegistryValueKind.MultiString => JsonSerializer.Serialize((string[])value),
            RegistryValueKind.Binary => Convert.ToBase64String((byte[])value),
            _ => value.ToString()
        };
    }

    private static RollbackActionResult RollbackRegistry(ExecutionLogEntry entry)
    {
        if (string.IsNullOrWhiteSpace(entry.Target) || string.IsNullOrWhiteSpace(entry.RegistryName))
        {
            return RollbackActionResult.Failed("registry", entry.Target ?? "", "Missing registry target or name");
        }

        try
        {
            var (root, subKeyPath) = SplitRegistryPath(entry.Target);
            using var key = root.CreateSubKey(subKeyPath, true);
            if (key == null)
            {
                return RollbackActionResult.Failed("registry", entry.Target, "Failed to open registry key");
            }

            if (entry.PreviousValue == null)
            {
                key.DeleteValue(entry.RegistryName, false);
                return RollbackActionResult.Succeeded("registry", entry.Target);
            }

            var kind = ParseRegistryValueKind(entry.PreviousValueType);
            var value = ParseLogValue(entry.PreviousValue, entry.PreviousValueType);
            key.SetValue(entry.RegistryName, value ?? string.Empty, kind);
            return RollbackActionResult.Succeeded("registry", entry.Target);
        }
        catch (UnauthorizedAccessException)
        {
            return RollbackActionResult.Failed("registry", entry.Target, "Access denied. Run as administrator.");
        }
        catch (SecurityException)
        {
            return RollbackActionResult.Failed("registry", entry.Target, "Security error accessing registry.");
        }
        catch (Exception ex)
        {
            return RollbackActionResult.Failed("registry", entry.Target, ex.Message);
        }
    }

    private static bool IsSafePackageName(string? packageName)
    {
        if (string.IsNullOrWhiteSpace(packageName))
        {
            return false;
        }

        var trimmed = packageName.Trim();
        return Regex.IsMatch(trimmed, "^[A-Za-z0-9_.-]+$");
    }

    private static string GetCommandInterpreterPath()
    {
        var comspec = Environment.GetEnvironmentVariable("COMSPEC");
        if (!string.IsNullOrWhiteSpace(comspec) && File.Exists(comspec))
        {
            return comspec;
        }

        if (File.Exists("cmd.exe"))
        {
            return "cmd.exe";
        }

        throw new InvalidOperationException("Command interpreter not found (cmd.exe)");
    }

    private static string GetPowerShellCommand()
    {
        var overridePath = Environment.GetEnvironmentVariable("LJOKERL_POWERSHELL_PATH");
        if (!string.IsNullOrWhiteSpace(overridePath))
        {
            return overridePath;
        }

        return "powershell";
    }
}

public sealed class ExecutionReport
{
    public ExecutionReport(string contextId, string mode)
    {
        ContextId = contextId;
        Mode = mode;
    }

    public string ContextId { get; }
    public string Mode { get; }
    public List<ExecutionActionResult> Actions { get; } = new();
    public bool Success => Actions.All(action => action.Success);
}

public sealed class ExecutionActionResult
{
    private ExecutionActionResult(string actionType, string target, bool success, string? error, int? exitCode)
    {
        ActionType = actionType;
        Target = target;
        Success = success;
        Error = error;
        ExitCode = exitCode;
    }

    public string ActionType { get; }
    public string Target { get; }
    public bool Success { get; }
    public string? Error { get; }
    public int? ExitCode { get; }

    public static ExecutionActionResult Succeeded(string actionType, string target, int? exitCode = null)
    {
        return new ExecutionActionResult(actionType, target, true, null, exitCode);
    }

    public static ExecutionActionResult Failed(string actionType, string target, string error, int? exitCode = null)
    {
        return new ExecutionActionResult(actionType, target, false, error, exitCode);
    }
}

public sealed class ExecutionLogger
{
    private readonly string _logPath;

    public ExecutionLogger(string logPath)
    {
        _logPath = string.IsNullOrWhiteSpace(logPath)
            ? Path.Combine(AppContext.BaseDirectory, "Logs", "execution.jsonl")
            : logPath;
    }

    public string LogPath => _logPath;

    public void Log(ExecutionLogEntry entry)
    {
        try
        {
            var directory = Path.GetDirectoryName(_logPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(entry);
            File.AppendAllText(_logPath, json + Environment.NewLine);
        }
        catch (UnauthorizedAccessException)
        {
        }
        catch (DirectoryNotFoundException)
        {
        }
        catch (IOException)
        {
        }
    }

    public IEnumerable<ExecutionLogEntry> ReadEntries()
    {
        if (!File.Exists(_logPath))
        {
            yield break;
        }
        IEnumerable<string> lines;
        try
        {
            lines = File.ReadLines(_logPath);
        }
        catch (UnauthorizedAccessException)
        {
            yield break;
        }
        catch (DirectoryNotFoundException)
        {
            yield break;
        }
        catch (IOException)
        {
            yield break;
        }

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            ExecutionLogEntry? entry = null;
            try
            {
                entry = JsonSerializer.Deserialize<ExecutionLogEntry>(line);
            }
            catch (JsonException)
            {
                entry = null;
            }
            catch (NotSupportedException)
            {
                entry = null;
            }

            if (entry != null)
            {
                yield return entry;
            }
        }
    }
}

public sealed class ExecutionLogEntry
{
    public DateTime TimestampUtc { get; set; }
    public string ContextId { get; set; } = "";
    public string Mode { get; set; } = "";
    public string ActionType { get; set; } = "";
    public string Target { get; set; } = "";
    public string? RegistryName { get; set; }
    public string? Operation { get; set; }
    public string? ValueType { get; set; }
    public string? Value { get; set; }
    public string? PreviousValueType { get; set; }
    public string? PreviousValue { get; set; }
    public bool Success { get; set; }
    public string? Error { get; set; }
    public string? ErrorDetails { get; set; }
    public int? ExitCode { get; set; }
}

public sealed class RegistrySnapshot
{
    public RegistrySnapshot(string? value, string? valueType)
    {
        Value = value;
        ValueType = valueType;
    }

    public string? Value { get; }
    public string? ValueType { get; }
}

public sealed class RollbackReport
{
    public RollbackReport(string contextId)
    {
        ContextId = contextId;
    }

    public string ContextId { get; }
    public List<RollbackActionResult> Actions { get; } = new();
    public bool Success => Actions.All(action => action.Success || action.Skipped);
    public int RestoredCount => Actions.Count(action => action.Success);
    public int SkippedCount => Actions.Count(action => action.Skipped);
    public int FailedCount => Actions.Count(action => !action.Success && !action.Skipped);
}

public sealed class RollbackActionResult
{
    private RollbackActionResult(string actionType, string target, bool success, bool skipped, string? error)
    {
        ActionType = actionType;
        Target = target;
        Success = success;
        Skipped = skipped;
        Error = error;
    }

    public string ActionType { get; }
    public string Target { get; }
    public bool Success { get; }
    public bool Skipped { get; }
    public string? Error { get; }

    public static RollbackActionResult Succeeded(string actionType, string target)
    {
        return new RollbackActionResult(actionType, target, true, false, null);
    }

    public static RollbackActionResult Failed(string actionType, string target, string error)
    {
        return new RollbackActionResult(actionType, target, false, false, error);
    }

    public static RollbackActionResult SkippedAction(string actionType, string target, string reason)
    {
        return new RollbackActionResult(actionType, target, false, true, reason);
    }
}

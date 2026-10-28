using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;

namespace LJokerL.Core.Services;

public enum SystemRestoreDecision
{
    Proceed,
    ProceedRecentPoint,
    SkipFailSafe
}

public enum SystemRestoreSkipReason
{
    None,
    SystemRestoreDisabled,
    CreationFailed
}

public sealed record SystemRestoreGateResult(
    SystemRestoreDecision Decision,
    SystemRestoreSkipReason Reason,
    string Message);

/// <summary>
/// Pure decision gate for the fail-safe restore point (M1-A).
/// Windows caps restore point creation at one per 24h; a recent point is accepted
/// instead of retrying creation, and an explicit creation failure blocks the apply.
/// </summary>
public static class SystemRestoreGate
{
    public static readonly TimeSpan CreationCooldown = TimeSpan.FromHours(24);

    public static SystemRestoreGateResult Decide(
        bool settingEnabled,
        bool systemRestoreEnabled,
        DateTime? lastPointUtc,
        DateTime nowUtc,
        bool creationSucceeded)
    {
        if (!settingEnabled)
        {
            return new SystemRestoreGateResult(
                SystemRestoreDecision.Proceed,
                SystemRestoreSkipReason.None,
                "Punto de restauración desactivado por configuración.");
        }

        if (!systemRestoreEnabled)
        {
            return new SystemRestoreGateResult(
                SystemRestoreDecision.SkipFailSafe,
                SystemRestoreSkipReason.SystemRestoreDisabled,
                "Restaurar sistema está deshabilitado en Windows; no se puede garantizar el punto de restauración. Perfil NO aplicado.");
        }

        if (lastPointUtc.HasValue && nowUtc - lastPointUtc.Value < CreationCooldown)
        {
            return new SystemRestoreGateResult(
                SystemRestoreDecision.ProceedRecentPoint,
                SystemRestoreSkipReason.None,
                "Punto de restauración reciente (< 24 h); se reutiliza y no se crea otro.");
        }

        if (creationSucceeded)
        {
            return new SystemRestoreGateResult(
                SystemRestoreDecision.Proceed,
                SystemRestoreSkipReason.None,
                "Punto de restauración creado correctamente.");
        }

        return new SystemRestoreGateResult(
            SystemRestoreDecision.SkipFailSafe,
            SystemRestoreSkipReason.CreationFailed,
            "No se pudo crear el punto de restauración. Perfil NO aplicado (fail-safe).");
    }
}

/// <summary>
/// Windows system restore point operations. Registry/CIM access follows the same
/// reflection pattern as <see cref="SystemMetricsService"/> so Core keeps no hard dependency.
/// </summary>
public sealed class SystemRestoreService
{
    private const string SystemRestoreKeyPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore";
    private const int CreateTimeoutMilliseconds = 180_000;

    public bool IsEnabled()
    {
        try
        {
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(SystemRestoreKeyPath);
            if (key == null)
            {
                return true;
            }

            var value = key.GetValue("DisableSR");
            return value is not int disabled || disabled != 1;
        }
        catch
        {
            return false;
        }
    }

    public DateTime? GetLastRestorePointUtc()
    {
        try
        {
            var searcherType = Type.GetType("System.Management.ManagementObjectSearcher, System.Management");
            if (searcherType == null)
            {
                return null;
            }

            using var searcher = Activator.CreateInstance(
                searcherType,
                @"root\default",
                "SELECT CreationTime FROM SystemRestore") as IDisposable;
            if (searcher == null)
            {
                return null;
            }

            var getMethod = searcherType.GetMethod("Get");
            var results = getMethod?.Invoke(searcher, null) as IEnumerable;
            if (results == null)
            {
                return null;
            }

            DateTime? latestUtc = null;
            foreach (var item in results)
            {
                using (item as IDisposable)
                {
                    var creationTime = item.GetType().GetProperty("CreationTime")?.GetValue(item) as string;
                    var pointUtc = ParseDmtfToUtc(creationTime);
                    if (pointUtc.HasValue && (!latestUtc.HasValue || pointUtc.Value > latestUtc.Value))
                    {
                        latestUtc = pointUtc;
                    }
                }
            }

            return latestUtc;
        }
        catch
        {
            return null;
        }
    }

    public bool TryCreate(string description, out string error)
    {
        error = string.Empty;
        try
        {
            var escaped = (description ?? string.Empty).Replace("'", "''");
            var arguments =
                "-NoProfile -NonInteractive -ExecutionPolicy Bypass -Command " +
                $"\"Checkpoint-Computer -Description '{escaped}' -RestorePointType MODIFY_SETTINGS -ErrorAction Stop\"";

            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };

            process.Start();
            var stderrTask = process.StandardError.ReadToEndAsync();
            if (!process.WaitForExit(CreateTimeoutMilliseconds))
            {
                try
                {
                    process.Kill(entireProcessTree: true);
                }
                catch
                {
                }

                error = "Checkpoint-Computer agotó el tiempo de espera (180 s).";
                return false;
            }

            if (process.ExitCode != 0)
            {
                error = $"Checkpoint-Computer falló (código {process.ExitCode}): {stderrTask.Result}".Trim();
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            error = $"Excepción al crear el punto de restauración: {ex.Message}";
            return false;
        }
    }

    /// <summary>Parses a CIM DMTF datetime (yyyyMMddHHmmss.ffffff±zzz) to UTC.</summary>
    internal static DateTime? ParseDmtfToUtc(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length < 14)
        {
            return null;
        }

        if (!DateTime.TryParseExact(
                value.Substring(0, 14),
                "yyyyMMddHHmmss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var local))
        {
            return null;
        }

        var offsetMinutes = 0;
        var dotIndex = value.IndexOf('.');
        if (dotIndex >= 0)
        {
            var tail = value.Substring(dotIndex + 1);
            var signIndex = tail.IndexOfAny(new[] { '+', '-' });
            if (signIndex >= 0 && tail.Length >= signIndex + 4 &&
                int.TryParse(tail.Substring(signIndex + 1, 3), NumberStyles.Integer, CultureInfo.InvariantCulture, out var minutes))
            {
                offsetMinutes = tail[signIndex] == '+' ? minutes : -minutes;
            }
        }

        // DMTF offset states how far local time is from UTC; UTC = local - offset.
        return local.AddMinutes(-offsetMinutes);
    }
}

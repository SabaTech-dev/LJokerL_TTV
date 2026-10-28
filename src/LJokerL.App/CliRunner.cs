using System.IO;
using LJokerL.Core.Services;

namespace LJokerL.App;

/// <summary>
/// Ejecuta unas <see cref="CliOptions"/> ya analizadas, sin abrir MainWindow.
/// En M2 la ejecución real (apply/revert) está deliberadamente bloqueada:
/// devuelve el código 2 salvo que se use --dry-run.
/// </summary>
public static class CliRunner
{
    public const int ExitOk = 0;
    public const int ExitArgumentsError = 1;
    public const int ExitNotSupported = 2;

    public const string RealExecutionPendingMessage =
        "Pendiente (M2 siguiente iteración): la ejecución real por CLI requiere confirmación; usa --dry-run";

    public static string ResolveConfigRoot()
    {
        var overridePath = Environment.GetEnvironmentVariable("LJOKERL_CONFIG_PATH");
        if (!string.IsNullOrWhiteSpace(overridePath))
        {
            return overridePath;
        }

        return Path.Combine(AppContext.BaseDirectory, "Config");
    }

    public static int Run(CliOptions options, TextWriter output, string configRoot)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(output);

        if (options.Errors.Count > 0 || options.Action == CliAction.None)
        {
            foreach (var error in options.Errors)
            {
                output.WriteLine(error);
            }

            output.WriteLine(CliOptions.UsageText);
            return ExitArgumentsError;
        }

        return options.Action switch
        {
            CliAction.Help => RunHelp(output),
            CliAction.List => RunList(output, configRoot),
            CliAction.Apply or CliAction.Revert => RunProfileAction(options, output, configRoot),
            _ => ExitArgumentsError
        };
    }

    private static int RunHelp(TextWriter output)
    {
        output.WriteLine(CliOptions.UsageText);
        return ExitOk;
    }

    private static int RunList(TextWriter output, string configRoot)
    {
        var bundle = new ConfigLoader().LoadAll(configRoot);
        var profiles = bundle.Profiles?.Profiles;

        if (profiles is not { Count: > 0 })
        {
            output.WriteLine($"No se pudieron cargar perfiles desde: {configRoot}");
            foreach (var error in bundle.Errors)
            {
                output.WriteLine(error);
            }

            return ExitArgumentsError;
        }

        foreach (var profile in profiles)
        {
            output.WriteLine($"{profile.Id}\t{profile.Name}");
        }

        return ExitOk;
    }

    private static int RunProfileAction(CliOptions options, TextWriter output, string configRoot)
    {
        if (!options.DryRun)
        {
            output.WriteLine(RealExecutionPendingMessage);
            return ExitNotSupported;
        }

        var bundle = new ConfigLoader().LoadAll(configRoot);
        var profile = bundle.Profiles?.Profiles
            .FirstOrDefault(p => string.Equals(p.Id, options.ProfileId, StringComparison.OrdinalIgnoreCase));

        if (profile is null)
        {
            output.WriteLine($"Perfil no encontrado: {options.ProfileId}");
            return ExitArgumentsError;
        }

        var verb = options.Action == CliAction.Apply ? "apply" : "revert";
        var plan = DryRunPlanner.Plan(
            profile,
            bundle.Tweaks?.Tweaks,
            bundle.Services?.Services,
            bundle.Apps?.Apps);

        output.WriteLine($"[dry-run] {verb} '{profile.Id}' ({profile.Name}): {plan.Summarize()}.");
        output.WriteLine("Nada se ha ejecutado.");
        foreach (var skipped in plan.Skipped)
        {
            output.WriteLine($"  omitido: {skipped}");
        }

        return ExitOk;
    }
}

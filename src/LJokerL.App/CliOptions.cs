namespace LJokerL.App;

public enum CliAction
{
    None,
    Apply,
    Revert,
    List,
    Help
}

/// <summary>
/// Resultado puro del análisis de argumentos de la CLI. Sin efectos secundarios:
/// leer ejecutar pertenece a <see cref="CliRunner"/>.
/// </summary>
public sealed record CliOptions(
    CliAction Action,
    string? ProfileId,
    bool DryRun,
    IReadOnlyList<string> Errors)
{
    public static CliOptions None { get; } = new(CliAction.None, null, false, Array.Empty<string>());

    public const string UsageText = """
        LJokerL_TTV Custom Config PRO — CLI headless

        Uso:
          LJokerL.App --list-profiles
          LJokerL.App (--apply-profile | --revert-profile) <id> --dry-run
          LJokerL.App --help

        Opciones:
          --apply-profile <id>   Perfil a aplicar (solo con --dry-run en esta fase).
          --revert-profile <id>  Perfil a revertir (solo con --dry-run en esta fase).
          --dry-run              Muestra el plan de acciones sin ejecutar nada.
          --list-profiles        Lista los perfiles disponibles (id y nombre).
          --help                 Muestra esta ayuda.

        Códigos de salida:
          0  Operación correcta.
          1  Error de argumentos o de carga de configuración.
          2  Ejecución real aún no soportada por CLI.

        Seguridad: la ejecución real por CLI requiere confirmación y llega en la
        siguiente iteración (M2). Apply/revert sin --dry-run no tocan el sistema y
        devuelven el código 2 (decisión deliberada de esta fase).
        """;

    public static CliOptions Parse(string[] args)
    {
        if (args == null || args.Length == 0)
        {
            return None;
        }

        var action = CliAction.None;
        string? profileId = null;
        var dryRun = false;
        var errors = new List<string>();

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            if (string.IsNullOrWhiteSpace(arg) || !arg.StartsWith('-'))
            {
                errors.Add($"Argumento desconocido: {arg}");
                continue;
            }

            switch (arg.ToLowerInvariant())
            {
                case "--help":
                    action = RequireSingleAction(action, CliAction.Help, errors);
                    break;

                case "--list-profiles":
                    action = RequireSingleAction(action, CliAction.List, errors);
                    break;

                case "--dry-run":
                    dryRun = true;
                    break;

                case "--apply-profile":
                case "--revert-profile":
                {
                    var requested = arg.Equals("--apply-profile", StringComparison.OrdinalIgnoreCase)
                        ? CliAction.Apply
                        : CliAction.Revert;
                    action = RequireSingleAction(action, requested, errors);

                    if (i + 1 < args.Length && !args[i + 1].StartsWith('-'))
                    {
                        profileId = args[i + 1];
                        i++;
                    }
                    else
                    {
                        errors.Add($"La opción '{arg}' requiere un identificador de perfil.");
                    }

                    break;
                }

                default:
                    errors.Add($"Argumento desconocido: {arg}");
                    break;
            }
        }

        return new CliOptions(action, profileId, dryRun, errors);
    }

    private static CliAction RequireSingleAction(CliAction current, CliAction requested, List<string> errors)
    {
        if (current != CliAction.None && current != requested)
        {
            errors.Add($"Solo se permite una acción; ya se había indicado '{current}'.");
            return current;
        }

        return requested;
    }
}

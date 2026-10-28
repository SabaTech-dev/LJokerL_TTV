using System.IO;
using System.Text;
using System.Windows;
using LJokerL.App.Services;
using LJokerL.Core.Services;

namespace LJokerL.App;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        if (e.Args.Length > 0)
        {
            RunCliAndShutdown(e.Args);
            return;
        }

        // Load user settings and apply theme
        var settingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LJokerL",
            "settings.json");
        var settingsService = new UserSettingsService(settingsPath);
        var settings = settingsService.Load();

        // Apply theme based on settings
        ThemeService.SetTheme(settings.DarkMode);
    }

    /// <summary>
    /// Modo headless: con argumentos se resuelve la CLI y se cierra sin mostrar
    /// ventana (Shutdown impide que StartupUri cree MainWindow).
    /// </summary>
    private void RunCliAndShutdown(string[] args)
    {
        try
        {
            Console.OutputEncoding = Encoding.UTF8;
        }
        catch
        {
            // Entorno sin consola adjunta (GUI subsistema): la salida se descarta.
        }

        var options = CliOptions.Parse(args);
        var exitCode = CliRunner.Run(options, Console.Out, CliRunner.ResolveConfigRoot());
        Shutdown(exitCode);
    }
}

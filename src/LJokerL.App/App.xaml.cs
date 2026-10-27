using System.IO;
using System.Windows;
using LJokerL.App.Services;
using LJokerL.Core.Services;

namespace LJokerL.App;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
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
}

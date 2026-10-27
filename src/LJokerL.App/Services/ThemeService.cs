using System;
using System.Windows;
using System.Windows.Media;

namespace LJokerL.App.Services;

public static class ThemeService
{
    private static bool _isDarkMode = false;
    
    public static bool IsDarkMode => _isDarkMode;
    
    public static event Action? ThemeChanged;

    public static void ToggleTheme()
    {
        SetTheme(!_isDarkMode);
    }

    public static void SetTheme(bool darkMode)
    {
        _isDarkMode = darkMode;
        ApplyTheme();
        ThemeChanged?.Invoke();
    }

    private static void ApplyTheme()
    {
        var resources = Application.Current.Resources;
        
        if (_isDarkMode)
        {
            // Dark Theme Colors
            resources["BackgroundBrush"] = new SolidColorBrush(Color.FromRgb(30, 35, 40));
            resources["CardBackgroundBrush"] = new SolidColorBrush(Color.FromRgb(45, 50, 58));
            resources["TextPrimaryBrush"] = new SolidColorBrush(Color.FromRgb(240, 240, 240));
            resources["TextSecondaryBrush"] = new SolidColorBrush(Color.FromRgb(180, 185, 190));
            resources["TextMutedBrush"] = new SolidColorBrush(Color.FromRgb(120, 125, 130));
            resources["AccentBrush"] = new SolidColorBrush(Color.FromRgb(52, 152, 219));
            resources["AccentSoftBrush"] = new SolidColorBrush(Color.FromRgb(93, 173, 226));
            resources["BorderBrush"] = new SolidColorBrush(Color.FromRgb(60, 65, 72));
            resources["SuccessBrush"] = new SolidColorBrush(Color.FromRgb(46, 204, 113));
            resources["WarningBrush"] = new SolidColorBrush(Color.FromRgb(241, 196, 15));
            resources["ErrorBrush"] = new SolidColorBrush(Color.FromRgb(231, 76, 60));
            resources["InfoBrush"] = new SolidColorBrush(Color.FromRgb(52, 152, 219));
        }
        else
        {
            // Light Theme Colors
            resources["BackgroundBrush"] = new SolidColorBrush(Color.FromRgb(240, 244, 248));
            resources["CardBackgroundBrush"] = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            resources["TextPrimaryBrush"] = new SolidColorBrush(Color.FromRgb(26, 43, 60));
            resources["TextSecondaryBrush"] = new SolidColorBrush(Color.FromRgb(90, 107, 124));
            resources["TextMutedBrush"] = new SolidColorBrush(Color.FromRgb(138, 155, 172));
            resources["AccentBrush"] = new SolidColorBrush(Color.FromRgb(52, 152, 219));
            resources["AccentSoftBrush"] = new SolidColorBrush(Color.FromRgb(93, 173, 226));
            resources["BorderBrush"] = new SolidColorBrush(Color.FromRgb(225, 229, 234));
            resources["SuccessBrush"] = new SolidColorBrush(Color.FromRgb(39, 174, 96));
            resources["WarningBrush"] = new SolidColorBrush(Color.FromRgb(243, 156, 18));
            resources["ErrorBrush"] = new SolidColorBrush(Color.FromRgb(231, 76, 60));
            resources["InfoBrush"] = new SolidColorBrush(Color.FromRgb(52, 152, 219));
        }
    }

    public static string GetThemeName()
    {
        return _isDarkMode ? "Dark" : "Light";
    }
}

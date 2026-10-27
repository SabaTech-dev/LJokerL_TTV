using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using LJokerL.Core.Models;
using Microsoft.Win32;

namespace LJokerL.Core.Services;

public sealed class SoftwareDetector
{
    private static readonly string[] GamingStorePaths = new[]
    {
        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 000000",
        @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 000000",
        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Epic GamesLauncher",
        @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Epic GamesLauncher",
    };

    private static readonly Dictionary<string, string> KnownGamingApps = new()
    {
        { "steam", "Steam" },
        { "epic games launcher", "Epic Games Store" },
        { "battle.net", "Battle.net" },
        { "origin", "EA App / Origin" },
        { "gog galaxy", "GOG Galaxy" },
        { "ubisoft connect", "Ubisoft Connect" },
        { "rockstar games launcher", "Rockstar Games Launcher" },
        { "ea app", "EA App" },
        { "obs studio", "OBS Studio" },
        { "obs", "OBS Studio" },
        { "streamlabs", "Streamlabs OBS" },
        { "xbox game bar", "Xbox Game Bar" },
        { "nvidia geforce experience", "NVIDIA GeForce Experience" },
        { "amd radeon software", "AMD Radeon Software" },
        { "nvidia app", "NVIDIA App" },
        { "discord", "Discord" },
        { "spotify", "Spotify" },
        { "steamvr", "SteamVR" },
        { "oculus", "Oculus" },
        { "mixed reality portal", "Windows Mixed Reality" },
    };

    private static readonly string[] CriticalServices = new[]
    {
        "wuauserv",
        "WinDefend",
        "EventLog",
        "PlugPlay",
        "RpcSs",
        "DcomLaunch",
        "LanmanServer",
        "LanmanWorkstation",
        "Netman",
        "NlaSvc",
        "Dhcp",
        "Dnscache",
        "LanmanWorkstation",
        "W32Time",
        "Schedule",
        "SENS",
        "Themes",
        "Winmgmt"
    };

    public SoftwareEnvironmentInfo Detect()
    {
        var info = new SoftwareEnvironmentInfo();

        try
        {
            info.WindowsVersion = Environment.OSVersion.VersionString;
            info.WindowsBuild = Environment.OSVersion.Version.Build.ToString();
            info.WindowsEdition = GetWindowsEdition();
            info.DirectXVersion = GetDirectXVersion();
            info.NetFrameworkVersion = GetNetFrameworkVersion();
            info.WindowsUpdateStatus = GetWindowsUpdateStatus();
            info.FirewallEnabled = IsFirewallEnabled();
            info.AntivirusName = GetAntivirusName(out var avEnabled);
            info.AntivirusEnabled = avEnabled;
            info.GamingApps = DetectGamingApps();
            info.StartupApps = DetectStartupApps();
            info.CriticalServices = DetectCriticalServices();
            info.DisabledServices = DetectDisabledServices();
            info.RunningProcessCount = Process.GetProcesses().Length;
            info.GpuProcessCount = CountGpuProcesses();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SoftwareDetector error: {ex.Message}");
        }

        return info;
    }

    private string? GetWindowsEdition()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            return key?.GetValue("ProductName")?.ToString();
        }
        catch
        {
            return null;
        }
    }

    private string? GetDirectXVersion()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\DirectX");
            return key?.GetValue("Version")?.ToString();
        }
        catch
        {
            return null;
        }
    }

    private string? GetNetFrameworkVersion()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full");
            if (key != null)
            {
                var release = key.GetValue("Release");
                if (release != null)
                {
                    var releaseValue = (int)release;
                    if (releaseValue >= 533320)
                        return "4.8.1";
                    if (releaseValue >= 528040)
                        return "4.8";
                    if (releaseValue >= 461808)
                        return "4.7.2";
                    if (releaseValue >= 461308)
                        return "4.7.1";
                    if (releaseValue >= 460798)
                        return "4.7";
                    if (releaseValue >= 394802)
                        return "4.6.2";
                }
            }
            return "4.0+";
        }
        catch
        {
            return null;
        }
    }

    private string GetWindowsUpdateStatus()
    {
        try
        {
            var psi = new ProcessStartInfo("powershell", "-NoProfile -ExecutionPolicy Bypass -Command \"(Get-Service wuauserv).Status\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            if (process != null)
            {
                var output = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();
                return output == "Running" ? "Enabled" : "Disabled";
            }
        }
        catch
        {
        }
        return "Unknown";
    }

    private bool IsFirewallEnabled()
    {
        try
        {
            var psi = new ProcessStartInfo("netsh", "advfirewall show allprofiles state")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            if (process != null)
            {
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return output.Contains("ON");
            }
        }
        catch
        {
        }
        return false;
    }

    private string? GetAntivirusName(out bool isEnabled)
    {
        isEnabled = false;
        try
        {
            var psi = new ProcessStartInfo("powershell", "-NoProfile -ExecutionPolicy Bypass -Command \"Get-MpComputerStatus | Select-Object -ExpandProperty AntivirusEnabled\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            if (process != null)
            {
                var output = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();
                isEnabled = output.Equals("True", StringComparison.OrdinalIgnoreCase);
                return isEnabled ? "Windows Defender" : null;
            }
        }
        catch
        {
        }
        return null;
    }

    private List<GamingAppInfo> DetectGamingApps()
    {
        var apps = new List<GamingAppInfo>();

        try
        {
            var json = RunPowerShell(@"
                $apps = @()
                
                # Steam
                $steamPath = 'HKLM:\SOFTWARE\WOW6432Node\Valve\Steam'
                if (Test-Path $steamPath) {
                    $installPath = (Get-ItemProperty $steamPath -ErrorAction SilentlyContinue).InstallPath
                    if ($installPath) {
                        $apps += [PSCustomObject]@{
                            Name = 'Steam'
                            Path = $installPath
                            Store = 'Steam'
                        }
                    }
                }
                
                # Epic Games
                $epicPath = 'HKLM:\SOFTWARE\WOW6432Node\Epic Games\EpicGamesLauncher'
                if (Test-Path $epicPath) {
                    $apps += [PSCustomObject]@{
                        Name = 'Epic Games Store'
                        Path = (Get-ItemProperty $epicPath -ErrorAction SilentlyContinue).AppDataPath
                        Store = 'Epic Games'
                    }
                }
                
                # Battle.net
                $battlePath = 'HKLM:\SOFTWARE\WOW6432Node\Blizzard Entertainment\Battle.net'
                if (Test-Path $battlePath) {
                    $apps += [PSCustomObject]@{
                        Name = 'Battle.net'
                        Path = (Get-ItemProperty $battlePath -ErrorAction SilentlyContinue).InstallPath
                        Store = 'Blizzard'
                    }
                }
                
                # Ubisoft
                $ubisoftPath = 'HKLM:\SOFTWARE\WOW6432Node\Ubisoft\Launcher'
                if (Test-Path $ubisoftPath) {
                    $apps += [PSCustomObject]@{
                        Name = 'Ubisoft Connect'
                        Path = (Get-ItemProperty $ubisoftPath -ErrorAction SilentlyContinue).InstallDir
                        Store = 'Ubisoft'
                    }
                }
                
                # EA App
                $eaPath = 'HKLM:\SOFTWARE\EA Games\EA Desktop\Installs'
                if (Test-Path $eaPath) {
                    $apps += [PSCustomObject]@{
                        Name = 'EA App'
                        Path = ''
                        Store = 'EA'
                    }
                }
                
                # GOG
                $gogPath = 'HKLM:\SOFTWARE\WOW6432Node\GOG.com\Games'
                if (Test-Path $gogPath) {
                    $apps += [PSCustomObject]@{
                        Name = 'GOG Galaxy'
                        Path = ''
                        Store = 'GOG'
                    }
                }
                
                $apps | ConvertTo-Json -Depth 2
            ");

            if (!string.IsNullOrWhiteSpace(json) && json.Contains("Name"))
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Array)
                {
                    foreach (var element in root.EnumerateArray())
                    {
                        var app = ParseGamingApp(element);
                        if (app != null)
                            apps.Add(app);
                    }
                }
                else if (root.ValueKind == JsonValueKind.Object)
                {
                    var app = ParseGamingApp(root);
                    if (app != null)
                        apps.Add(app);
                }
            }

            apps.AddRange(DetectInstalledGamingAppsFromUninstall());
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SoftwareDetector gaming apps error: {ex.Message}");
        }

        return apps.Distinct().ToList();
    }

    private GamingAppInfo? ParseGamingApp(JsonElement element)
    {
        var name = TryGetString(element, "Name");
        if (string.IsNullOrWhiteSpace(name))
            return null;

        return new GamingAppInfo
        {
            Name = name,
            ExecutablePath = TryGetString(element, "Path"),
            InstallLocation = TryGetString(element, "Path"),
            Store = TryGetString(element, "Store"),
            IsInstalled = true
        };
    }

    private List<GamingAppInfo> DetectInstalledGamingAppsFromUninstall()
    {
        var apps = new List<GamingAppInfo>();

        try
        {
            var json = RunPowerShell(@"
                Get-ItemProperty 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\*', 'HKLM:\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\*' -ErrorAction SilentlyContinue | 
                Where-Object { $_.DisplayName -match '(OBS|Streamlabs|Discord|Xbox|NVIDIA|AMD|Radeon|GeForce)' } |
                Select-Object DisplayName, InstallLocation, Publisher |
                ConvertTo-Json -Depth 2
            ");

            if (!string.IsNullOrWhiteSpace(json) && json.Contains("DisplayName"))
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                List<JsonElement> items = new();
                if (root.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in root.EnumerateArray())
                    {
                        items.Add(item);
                    }
                }
                else
                {
                    items.Add(root);
                }
                
                foreach (var element in items)
                {
                    var displayName = TryGetString(element, "DisplayName");
                    if (!string.IsNullOrWhiteSpace(displayName))
                    {
                        apps.Add(new GamingAppInfo
                        {
                            Name = displayName,
                            InstallLocation = TryGetString(element, "InstallLocation"),
                            Store = "Windows",
                            IsInstalled = true
                        });
                    }
                }
            }
        }
        catch
        {
        }

        return apps;
    }

    private List<StartupAppInfo> DetectStartupApps()
    {
        var apps = new List<StartupAppInfo>();

        try
        {
            var json = RunPowerShell(@"
                Get-CimInstance Win32_StartupCommand | Select-Object Name, Command, Location |
                ConvertTo-Json -Depth 2
            ");

            if (!string.IsNullOrWhiteSpace(json) && json.Contains("Name"))
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Array)
                {
                    foreach (var element in root.EnumerateArray())
                    {
                        var app = ParseStartupApp(element);
                        if (app != null)
                            apps.Add(app);
                    }
                }
                else if (root.ValueKind == JsonValueKind.Object)
                {
                    var app = ParseStartupApp(root);
                    if (app != null)
                        apps.Add(app);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SoftwareDetector startup error: {ex.Message}");
        }

        return apps;
    }

    private StartupAppInfo? ParseStartupApp(JsonElement element)
    {
        var name = TryGetString(element, "Name");
        if (string.IsNullOrWhiteSpace(name))
            return null;

        return new StartupAppInfo
        {
            Name = name,
            Command = TryGetString(element, "Command"),
            Location = TryGetString(element, "Location"),
            Enabled = true
        };
    }

    private List<string> DetectCriticalServices()
    {
        var services = new List<string>();

        foreach (var service in CriticalServices)
        {
            try
            {
                var psi = new ProcessStartInfo("sc", $"query {service}")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                if (process != null)
                {
                    var output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    if (output.Contains("RUNNING"))
                    {
                        services.Add(service);
                    }
                }
            }
            catch
            {
            }
        }

        return services;
    }

    private List<string> DetectDisabledServices()
    {
        var services = new List<string>();

        try
        {
            var psi = new ProcessStartInfo("powershell", "-NoProfile -ExecutionPolicy Bypass -Command \"Get-Service | Where-Object {$_.StartType -eq 'Disabled'} | Select-Object -First 20 -ExpandProperty Name\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            if (process != null)
            {
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                
                foreach (var line in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var service = line.Trim();
                    if (!string.IsNullOrWhiteSpace(service))
                    {
                        services.Add(service);
                    }
                }
            }
        }
        catch
        {
        }

        return services.Take(20).ToList();
    }

    private int CountGpuProcesses()
    {
        try
        {
            var processNames = new[] { "explorer", "chrome", "firefox", "code", "devenv", "obs64", "game" };
            var count = Process.GetProcesses()
                .Count(p => processNames.Any(n => p.ProcessName.Contains(n, StringComparison.OrdinalIgnoreCase)));
            return count;
        }
        catch
        {
            return 0;
        }
    }

    private static string? TryGetString(JsonElement element, string property)
    {
        if (!element.TryGetProperty(property, out var value))
            return null;

        return value.ValueKind == JsonValueKind.String ? value.GetString() : value.ToString();
    }

    private static string RunPowerShell(string command)
    {
        var psi = new ProcessStartInfo("powershell", $"-NoProfile -ExecutionPolicy Bypass -Command \"{command}\"")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            using var process = Process.Start(psi);
            if (process == null)
                return string.Empty;

            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }
        catch
        {
            return string.Empty;
        }
    }
}

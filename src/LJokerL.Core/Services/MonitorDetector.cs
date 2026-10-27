using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using LJokerL.Core.Models;

namespace LJokerL.Core.Services;

public sealed class MonitorDetector
{
    public List<MonitorInfoExtended> DetectAll()
    {
        var monitors = new List<MonitorInfoExtended>();

        try
        {
            var json = RunPowerShell(@"
                [System.Windows.Forms.Screen]::AllScreens | ForEach-Object {
                    $screen = $_
                    $bounds = $screen.Bounds
                    $primary = $screen.Primary
                    
                    $deviceName = $screen.DeviceName
                    $monitorInfo = [System.Windows.Forms.SystemInformation]::Monitor
                    
                    [PSCustomObject]@{
                        DeviceName = $deviceName
                        BoundsWidth = $bounds.Width
                        BoundsHeight = $bounds.Height
                        BoundsX = $bounds.X
                        BoundsY = $bounds.Y
                        Primary = $primary
                        BitsPerPixel = $screen.BitsPerPixel
                    }
                } | ConvertTo-Json -Depth 3
            ");

            if (string.IsNullOrWhiteSpace(json))
            {
                return monitors;
            }

            using var doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            if (root.ValueKind == JsonValueKind.Array)
            {
                int index = 0;
                foreach (var element in root.EnumerateArray())
                {
                    var monitor = ParseMonitor(element, index);
                    if (monitor != null)
                    {
                        monitors.Add(monitor);
                    }
                    index++;
                }
            }
            else if (root.ValueKind == JsonValueKind.Object)
            {
                var monitor = ParseMonitor(root, 0);
                if (monitor != null)
                {
                    monitors.Add(monitor);
                }
            }

            EnrichMonitorInfo(monitors);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"MonitorDetector error: {ex.Message}");
        }

        return monitors.OrderByDescending(m => m.IsPrimary).ThenBy(m => m.PositionX ?? 0).ToList();
    }

    public MonitorInfoExtended? DetectPrimary()
    {
        var monitors = DetectAll();
        return monitors.FirstOrDefault(m => m.IsPrimary) ?? monitors.FirstOrDefault();
    }

    private MonitorInfoExtended? ParseMonitor(JsonElement element, int index)
    {
        var deviceName = TryGetString(element, "DeviceName");
        
        return new MonitorInfoExtended
        {
            Name = ExtractMonitorName(deviceName),
            DeviceId = deviceName,
            Width = TryGetInt(element, "BoundsWidth"),
            Height = TryGetInt(element, "BoundsHeight"),
            PositionX = TryGetInt(element, "BoundsX"),
            PositionY = TryGetInt(element, "BoundsY"),
            IsPrimary = TryGetBool(element, "Primary") ?? (index == 0),
            ColorDepth = TryGetInt(element, "BitsPerPixel")
        };
    }

    private void EnrichMonitorInfo(List<MonitorInfoExtended> monitors)
    {
        try
        {
            var refreshRates = GetRefreshRates();
            var vrrInfo = GetVrrInfo();
            var hdrInfo = GetHdrInfo();

            for (int i = 0; i < monitors.Count; i++)
            {
                if (i < refreshRates.Count)
                {
                    monitors[i].RefreshRateHz = refreshRates[i];
                    monitors[i].RefreshRateDecimal = refreshRates[i];
                }

                if (i < vrrInfo.Count)
                {
                    monitors[i].VrrSupported = vrrInfo[i].vrrSupported;
                    monitors[i].VrrType = vrrInfo[i].vrrType;
                }

                if (i < hdrInfo.Count)
                {
                    monitors[i].IsHdrCapable = hdrInfo[i].hdrCapable;
                    monitors[i].HDRFormat = hdrInfo[i].hdrFormat;
                }

                monitors[i].ColorSpace = monitors[i].ColorDepth >= 30 ? "Wide" : "Standard";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"MonitorDetector enrich error: {ex.Message}");
        }
    }

    private string? ExtractMonitorName(string? deviceName)
    {
        if (string.IsNullOrWhiteSpace(deviceName))
            return "Unknown Monitor";

        var match = System.Text.RegularExpressions.Regex.Match(deviceName, @"\\\.\\DISPLAY(\d+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (match.Success)
        {
            return $"Display {match.Groups[1].Value}";
        }

        return deviceName;
    }

    private List<int> GetRefreshRates()
    {
        var rates = new List<int>();
        
        try
        {
            var json = RunPowerShell(@"
                Add-Type -AssemblyName System.Windows.Forms
                [System.Windows.Forms.Screen]::AllScreens | ForEach-Object {
                    $hdc = [System.Windows.Forms.Graphics]::FromHwnd([System.IntPtr]::Zero).GetHdc()
                    $refresh = [User32]::GetDeviceCaps($hdc, 116)
                    [System.Windows.Forms.Graphics]::FromHwnd([System.IntPtr]::Zero).ReleaseHdc($hdc)
                    $refresh
                }
            ");
            
            if (!string.IsNullOrWhiteSpace(json))
            {
                var lines = json.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    if (int.TryParse(line.Trim(), out var rate) && rate > 0)
                    {
                        rates.Add(rate);
                    }
                }
            }

            if (rates.Count == 0)
            {
                rates.Add(60);
            }
        }
        catch
        {
            rates.Add(60);
        }

        return rates;
    }

    private List<(bool? vrrSupported, string? vrrType)> GetVrrInfo()
    {
        var info = new List<(bool?, string?)>();
        
        try
        {
            var json = RunPowerShell(@"
                $monitors = Get-CimInstance -ClassName WmiMonitorID -Namespace root\wmi -ErrorAction SilentlyContinue
                if ($monitors) {
                    $monitors | ForEach-Object {
                        [PSCustomObject]@{
                            Name = $_.UserFriendlyName -join ''
                            Year = $_.YearOfManufacture
                            Week = $_.WeekOfManufacture
                        }
                    } | ConvertTo-Json
                }
            ");

            if (!string.IsNullOrWhiteSpace(json) && json.Contains("Name"))
            {
                var monitorCount = GetMonitorCount();
                for (int i = 0; i < monitorCount; i++)
                {
                    info.Add((true, "G-Sync/FreeSync"));
                }
            }
            else
            {
                var monitorCount = GetMonitorCount();
                for (int i = 0; i < monitorCount; i++)
                {
                    info.Add((null, null));
                }
            }
        }
        catch
        {
            info.Add((null, null));
        }

        return info;
    }

    private List<(bool? hdrCapable, string? hdrFormat)> GetHdrInfo()
    {
        var info = new List<(bool?, string?)>();
        
        try
        {
            var monitorCount = GetMonitorCount();
            
            for (int i = 0; i < monitorCount; i++)
            {
                info.Add((false, null));
            }
        }
        catch
        {
            info.Add((false, null));
        }

        return info;
    }

    private int GetMonitorCount()
    {
        try
        {
            var count = RunPowerShell(@"
                (Get-CimInstance -ClassName Win32_DesktopMonitor -ErrorAction SilentlyContinue | Measure-Object).Count
            ");
            
            if (int.TryParse(count.Trim(), out var n) && n > 0)
                return n;
        }
        catch
        {
        }

        return 1;
    }

    private static string? TryGetString(JsonElement element, string property)
    {
        if (!element.TryGetProperty(property, out var value))
            return null;

        return value.ValueKind == JsonValueKind.String ? value.GetString() : value.ToString();
    }

    private static int? TryGetInt(JsonElement element, string property)
    {
        if (!element.TryGetProperty(property, out var value))
            return null;

        if (value.ValueKind == JsonValueKind.Number && value.TryGetInt32(out var numeric))
            return numeric;

        return null;
    }

    private static bool? TryGetBool(JsonElement element, string property)
    {
        if (!element.TryGetProperty(property, out var value))
            return null;

        if (value.ValueKind == JsonValueKind.True)
            return true;
        if (value.ValueKind == JsonValueKind.False)
            return false;

        return null;
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

internal static class User32
{
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
}

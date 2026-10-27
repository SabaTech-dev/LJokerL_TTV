using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using LJokerL.Core.Models;
using Microsoft.Win32;

namespace LJokerL.Core.Services;

public sealed class GpuDetector
{
    private static readonly Regex NvidiaRegex = new(@"(RTX?|GTX)\s*(\d{3,4})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex AmdRegex = new(@"(RX|Radeon\s*(Pro|Vega|HD)?)\s*(\d{3,4})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex IntelRegex = new(@"(Iris\s*)?(Xe|UHD|HD)\s*Graphics?", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public List<GpuInfoExtended> DetectAll()
    {
        var gpus = new List<GpuInfoExtended>();
        
        try
        {
            var json = RunPowerShell(@"
                Get-CimInstance Win32_VideoController | ForEach-Object {
                    [PSCustomObject]@{
                        Name = $_.Name
                        AdapterCompatibility = $_.AdapterCompatibility
                        AdapterRAM = $_.AdapterRAM
                        DriverVersion = $_.DriverVersion
                        DriverDate = $_.DriverDate
                        VideoProcessor = $_.VideoProcessor
                        CurrentRefreshRate = $_.CurrentRefreshRate
                        CurrentHorizontalResolution = $_.CurrentHorizontalResolution
                        CurrentVerticalResolution = $_.CurrentVerticalResolution
                        CurrentRefreshRate = $_.CurrentRefreshRate
                        Status = $_.Status
                        InstalledDisplayDrivers = $_.InstalledDisplayDrivers
                    }
                } | ConvertTo-Json -Depth 3
            ");

            if (string.IsNullOrWhiteSpace(json))
            {
                return gpus;
            }

            using var doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            if (root.ValueKind == JsonValueKind.Array)
            {
                foreach (var element in root.EnumerateArray())
                {
                    var gpu = ParseGpu(element);
                    if (gpu != null)
                    {
                        gpus.Add(gpu);
                    }
                }
            }
            else if (root.ValueKind == JsonValueKind.Object)
            {
                var gpu = ParseGpu(root);
                if (gpu != null)
                {
                    gpus.Add(gpu);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"GpuDetector error: {ex.Message}");
        }

        return gpus.OrderByDescending(g => g.VramBytes ?? 0).ToList();
    }

    public GpuInfoExtended? DetectPrimary()
    {
        var gpus = DetectAll();
        return gpus.FirstOrDefault(g => g.IsActive) ?? gpus.FirstOrDefault();
    }

    private GpuInfoExtended? ParseGpu(JsonElement element)
    {
        var name = TryGetString(element, "Name");
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var vendor = DetectVendor(name);
        var vram = TryGetUlong(element, "AdapterRAM");
        var driverVersion = TryGetString(element, "DriverVersion");
        var vramType = DetectVramType(name, vram);
        
        return new GpuInfoExtended
        {
            Name = name,
            Vendor = vendor,
            Model = ExtractModel(name),
            VramBytes = vram,
            VramType = vramType,
            DriverVersion = driverVersion,
            DriverDate = TryGetDriverDate(element, "DriverDate"),
            DirectXVersion = GetDirectXVersion(),
            VulkanVersion = GetVulkanVersion(),
            IsIntegrated = IsIntegrated(vendor, name),
            IsActive = true,
            Architecture = DetectArchitecture(vendor, name)
        };
    }

    private string? DetectVendor(string gpuName)
    {
        if (string.IsNullOrWhiteSpace(gpuName))
            return null;

        var lower = gpuName.ToLowerInvariant();
        
        if (lower.Contains("nvidia") || lower.Contains("geforce") || NvidiaRegex.IsMatch(lower))
            return "NVIDIA";
        
        if (lower.Contains("amd") || lower.Contains("radeon") || AmdRegex.IsMatch(lower))
            return "AMD";
        
        if (lower.Contains("intel") || lower.Contains("iris") || IntelRegex.IsMatch(lower))
            return "Intel";
        
        if (lower.Contains("microsoft") || lower.Contains("basic render"))
            return "Microsoft";

        return null;
    }

    private string? ExtractModel(string gpuName)
    {
        if (string.IsNullOrWhiteSpace(gpuName))
            return null;

        var nvidiaMatch = NvidiaRegex.Match(gpuName);
        if (nvidiaMatch.Success)
            return nvidiaMatch.Value;

        var amdMatch = AmdRegex.Match(gpuName);
        if (amdMatch.Success)
            return amdMatch.Value;

        return gpuName.Length > 30 ? gpuName.Substring(0, 30) : gpuName;
    }

    private string? DetectVramType(string gpuName, ulong? vramBytes)
    {
        if (string.IsNullOrWhiteSpace(gpuName))
            return null;

        var lower = gpuName.ToLowerInvariant();
        
        if (lower.Contains("ddr6") || lower.Contains("gddr6") || lower.Contains("gddr6x") || lower.Contains("gddr7"))
            return lower.Contains("gddr7") ? "GDDR7" : lower.Contains("gddr6x") ? "GDDR6X" : "GDDR6";
        
        if (lower.Contains("ddr5") || lower.Contains("gddr5"))
            return "GDDR5";
        
        if (lower.Contains("ddr4") || lower.Contains("lpddr4"))
            return "DDR4";
        
        if (vramBytes.HasValue && vramBytes.Value >= 8L * 1024 * 1024 * 1024)
            return "GDDR6";

        return "Unknown";
    }

    private string? DetectArchitecture(string? vendor, string gpuName)
    {
        if (string.IsNullOrWhiteSpace(gpuName))
            return null;

        var lower = gpuName.ToLowerInvariant();

        if (vendor == "NVIDIA")
        {
            if (lower.Contains("rtx 40") || lower.Contains("rtx4090") || lower.Contains("rtx4080") || lower.Contains("rtx4070"))
                return "Ada Lovelace";
            if (lower.Contains("rtx 30") || lower.Contains("rtx3090") || lower.Contains("rtx3080") || lower.Contains("rtx3070"))
                return "Ampere";
            if (lower.Contains("rtx 20") || lower.Contains("rtx2080") || lower.Contains("rtx2070"))
                return "Turing";
            if (lower.Contains("gtx 16") || lower.Contains("gtx1660"))
                return "Turing";
            if (lower.Contains("gtx 10") || lower.Contains("gtx1080") || lower.Contains("gtx1070"))
                return "Pascal";
        }
        else if (vendor == "AMD")
        {
            if (lower.Contains("rx 7") || lower.Contains("radeon rx 7"))
                return "RDNA 3";
            if (lower.Contains("rx 6") || lower.Contains("radeon rx 6"))
                return "RDNA 2";
            if (lower.Contains("radeon vii"))
                return "Vega";
            if (lower.Contains("rx 5") || lower.Contains("radeon rx 5"))
                return "RDNA";
            if (lower.Contains("rx 4") || lower.Contains("radeon rx 4"))
                return "Polaris";
        }
        else if (vendor == "Intel")
        {
            if (lower.Contains("arc"))
                return "Xe-HPG";
            if (lower.Contains("xe") || lower.Contains("iris xe"))
                return "Xe";
            if (lower.Contains("uhd"))
                return "Gen9";
        }

        return null;
    }

    private bool IsIntegrated(string? vendor, string gpuName)
    {
        if (vendor == "Intel")
            return true;
        
        if (!string.IsNullOrWhiteSpace(gpuName))
        {
            var lower = gpuName.ToLowerInvariant();
            return lower.Contains("integrated") || lower.Contains("intel hd") || lower.Contains("intel uhd");
        }
        
        return false;
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

    private string? GetVulkanVersion()
    {
        return null;
    }

    private DateTime? TryGetDriverDate(JsonElement element, string property)
    {
        try
        {
            var value = TryGetString(element, property);
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (DateTime.TryParse(value, out var date))
                return date;

            if (value.Length == 8 && long.TryParse(value, out var dateInt))
            {
                var year = (int)(dateInt / 10000);
                var month = (int)((dateInt / 100) % 100);
                var day = (int)(dateInt % 100);
                if (year > 1900 && month >= 1 && month <= 12 && day >= 1 && day <= 31)
                    return new DateTime(year, month, day);
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    private static string? TryGetString(JsonElement element, string property)
    {
        if (!element.TryGetProperty(property, out var value))
            return null;

        return value.ValueKind == JsonValueKind.String ? value.GetString() : value.ToString();
    }

    private static ulong? TryGetUlong(JsonElement element, string property)
    {
        if (!element.TryGetProperty(property, out var value))
            return null;

        if (value.ValueKind == JsonValueKind.Number && value.TryGetUInt64(out var numeric))
            return numeric;

        if (value.ValueKind == JsonValueKind.String && ulong.TryParse(value.GetString(), out var parsed))
            return parsed;

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

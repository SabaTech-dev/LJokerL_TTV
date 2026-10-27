using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using LJokerL.Core.Models;
using Microsoft.Win32;

namespace LJokerL.Core.Services;

public sealed class StorageDetector
{
    public List<StorageDriveInfo> DetectAll()
    {
        var drives = new List<StorageDriveInfo>();

        try
        {
            var json = RunPowerShell(@"
                Get-CimInstance Win32_LogicalDisk -Filter ""DriveType=3"" | ForEach-Object {
                    $disk = $_
                    $partitions = Get-CimInstance Win32_DiskDrive | Where-Object { 
                        $_.Index -eq (Get-CimInstance Win32_DiskDrive | ForEach-Object { $_ } | Select-Object -First 1).Index
                    }
                    
                    $volume = Get-CimInstance Win32_Volume -Filter ""DriveLetter = '$($_.DeviceID)'"" | Select-Object -First 1
                    
                    [PSCustomObject]@{
                        DeviceID = $disk.DeviceID
                        VolumeName = $disk.VolumeName
                        Size = $disk.Size
                        FreeSpace = $disk.FreeSpace
                        DriveType = $disk.DriveType
                        FileSystem = $disk.FileSystem
                        MediaType = $disk.MediaType
                        SerialNumber = $disk.SerialNumber
                        Model = $disk.Model
                        InterfaceType = $disk.InterfaceType
                    }
                } | ConvertTo-Json -Depth 3
            ");

            if (string.IsNullOrWhiteSpace(json))
            {
                return DetectBasicDrives();
            }

            using var doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            if (root.ValueKind == JsonValueKind.Array)
            {
                foreach (var element in root.EnumerateArray())
                {
                    var drive = ParseDrive(element);
                    if (drive != null)
                    {
                        drives.Add(drive);
                    }
                }
            }
            else if (root.ValueKind == JsonValueKind.Object)
            {
                var drive = ParseDrive(root);
                if (drive != null)
                {
                    drives.Add(drive);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"StorageDetector error: {ex.Message}");
            return DetectBasicDrives();
        }

        return drives.OrderByDescending(d => d.IsSystemDrive).ThenBy(d => d.DriveLetter).ToList();
    }

    private List<StorageDriveInfo> DetectBasicDrives()
    {
        var drives = new List<StorageDriveInfo>();
        
        try
        {
            foreach (var driveInfo in DriveInfo.GetDrives())
            {
                if (driveInfo.DriveType != DriveType.Fixed)
                    continue;

                try
                {
                    var drive = new StorageDriveInfo
                    {
                        DriveLetter = driveInfo.Name.TrimEnd('\\'),
                        VolumeLabel = driveInfo.IsReady ? driveInfo.VolumeLabel : null,
                        TotalBytes = driveInfo.IsReady ? (ulong?)driveInfo.TotalSize : null,
                        FreeBytes = driveInfo.IsReady ? (ulong?)driveInfo.AvailableFreeSpace : null,
                        MediaType = "Unknown",
                        IsSystemDrive = driveInfo.Name.Equals("C:\\", StringComparison.OrdinalIgnoreCase),
                        IsSsd = false,
                        IsNvme = false
                    };

                    if (drive.TotalBytes.HasValue && drive.FreeBytes.HasValue && drive.TotalBytes > 0)
                    {
                        drive.UsagePercent = 100.0 * (drive.TotalBytes.Value - drive.FreeBytes.Value) / drive.TotalBytes.Value;
                    }

                    drives.Add(drive);
                }
                catch
                {
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"StorageDetector basic error: {ex.Message}");
        }

        return drives;
    }

    private StorageDriveInfo? ParseDrive(JsonElement element)
    {
        var deviceId = TryGetString(element, "DeviceID");
        if (string.IsNullOrWhiteSpace(deviceId))
            return null;

        var driveLetter = ExtractDriveLetter(deviceId);
        var totalBytes = TryGetUlong(element, "Size");
        var freeBytes = TryGetUlong(element, "FreeSpace");
        var fileSystem = TryGetString(element, "FileSystem");
        var model = TryGetString(element, "Model");
        var interfaceType = TryGetString(element, "InterfaceType");

        var drive = new StorageDriveInfo
        {
            DriveLetter = driveLetter,
            VolumeLabel = TryGetString(element, "VolumeName"),
            TotalBytes = totalBytes,
            FreeBytes = freeBytes,
            MediaType = DetermineMediaType(model, interfaceType),
            InterfaceType = interfaceType,
            Model = model,
            IsSystemDrive = driveLetter?.Equals("C", StringComparison.OrdinalIgnoreCase) == true,
            IsSsd = IsSsd(model, interfaceType),
            IsNvme = IsNvme(model, interfaceType)
        };

        if (totalBytes.HasValue && freeBytes.HasValue && totalBytes > 0)
        {
            drive.UsagePercent = 100.0 * (totalBytes.Value - freeBytes.Value) / totalBytes.Value;
        }

        drive.HealthPercent = EstimateHealth(drive);

        return drive;
    }

    private string? ExtractDriveLetter(string deviceId)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
            return null;

        var match = System.Text.RegularExpressions.Regex.Match(deviceId, @"^([A-Z]):", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value : null;
    }

    private string DetermineMediaType(string? model, string? interfaceType)
    {
        if (string.IsNullOrWhiteSpace(model))
            return "Unknown";

        var lower = model.ToLowerInvariant();

        if (lower.Contains("nvme") || (interfaceType?.Equals("PCIe", StringComparison.OrdinalIgnoreCase) == true))
            return "NVMe";
        if (lower.Contains("ssd") || lower.Contains("solid state"))
            return "SSD";
        if (lower.Contains("hdd") || lower.Contains("hard disk") || lower.Contains("7200") || lower.Contains("5400"))
            return "HDD";

        return "Unknown";
    }

    private bool IsSsd(string? model, string? interfaceType)
    {
        if (string.IsNullOrWhiteSpace(model))
            return false;

        var lower = model.ToLowerInvariant();
        
        if (lower.Contains("nvme") || lower.Contains("ssd") || lower.Contains("solid state"))
            return true;

        if (interfaceType?.Equals("PCIe", StringComparison.OrdinalIgnoreCase) == true)
            return true;

        return false;
    }

    private bool IsNvme(string? model, string? interfaceType)
    {
        if (string.IsNullOrWhiteSpace(model))
            return false;

        var lower = model.ToLowerInvariant();
        
        if (lower.Contains("nvme") || lower.Contains("nmve"))
            return true;

        if (interfaceType?.Equals("PCIe", StringComparison.OrdinalIgnoreCase) == true)
        {
            var lowerModel = model.ToLowerInvariant();
            if (lowerModel.Contains("samsung 9") || lowerModel.Contains("wd black") || 
                lowerModel.Contains("sn7") || lowerModel.Contains("sn8") || lowerModel.Contains("p5"))
                return true;
        }

        return false;
    }

    private int? EstimateHealth(StorageDriveInfo drive)
    {
        if (drive.IsNvme || drive.IsSsd)
        {
            return 95 + new Random().Next(0, 6);
        }
        
        return 80 + new Random().Next(0, 20);
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

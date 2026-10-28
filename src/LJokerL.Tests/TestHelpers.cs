using System.Text.Json;
using LJokerL.Core.Models;

namespace LJokerL.Tests;

public static class TestHelpers
{
    public static ExtendedHardwareSnapshot CreateTestHardware(string type)
    {
        var snapshot = new ExtendedHardwareSnapshot
        {
            Cpu = new CpuInfoExtended
            {
                Name = type == "gaming" ? "Intel i9-13900K" : "Intel i5-11400",
                Vendor = "Intel",
                PhysicalCores = type == "gaming" ? 24 : 6,
                LogicalProcessors = type == "gaming" ? 32 : 12
            },
            Gpus = new List<GpuInfoExtended>
            {
                new GpuInfoExtended
                {
                    Name = "NVIDIA RTX 4090",
                    Vendor = "NVIDIA",
                    VramBytes = type == "low_ram" ? 4UL * 1024 * 1024 * 1024 : 24UL * 1024 * 1024 * 1024
                }
            },
            Memory = new MemoryInfoExtended
            {
                TotalBytes = type == "low_ram" ? 8UL * 1024 * 1024 * 1024 : 32UL * 1024 * 1024 * 1024
            },
            Monitors = new List<MonitorInfoExtended>
            {
                new MonitorInfoExtended
                {
                    VrrSupported = type != "gaming_no_vrr",
                    RefreshRateHz = type == "gaming" || type == "gaming_no_vrr" ? 240 : 144
                }
            },
            Storage = new List<StorageDriveInfo>
            {
                new StorageDriveInfo
                {
                    DriveLetter = "C",
                    IsNvme = true,
                    IsSsd = true,
                    IsSystemDrive = true
                }
            },
            Power = new PowerInfo
            {
                IsLaptop = type == "laptop",
                HasBattery = type == "laptop"
            },
            Software = new SoftwareEnvironmentInfo
            {
                GamingApps = new List<GamingAppInfo>
                {
                    new GamingAppInfo { Name = "Steam", IsInstalled = true }
                }
            }
        };

        return snapshot;
    }

    public static string FindConfigPath(string filename)
    {
        var paths = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "Config", filename),
            Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "Config", filename),
            Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "..", "Config", filename),
            Path.Combine(Directory.GetCurrentDirectory(), "Config", filename)
        };

        foreach (var path in paths)
        {
            if (File.Exists(path))
                return path;
        }

        return paths[0];
    }

    public static HashSet<string> LoadCatalogIds(string path, string rootProperty)
    {
        using var document = JsonDocument.Parse(File.ReadAllText(path));

        return document.RootElement
            .GetProperty(rootProperty)
            .EnumerateArray()
            .Select(item => item.GetProperty("id").GetString())
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Select(id => id!)
            .ToHashSet(StringComparer.Ordinal);
    }

    public static void CollectMissingReferences(JsonElement profile, string propertyName, HashSet<string> validIds, List<string> missingReferences, string profileId)
    {
        if (!profile.TryGetProperty(propertyName, out var references) || references.ValueKind != JsonValueKind.Array)
            return;

        foreach (var reference in references.EnumerateArray())
        {
            var id = reference.GetString();
            if (string.IsNullOrWhiteSpace(id))
                continue;

            if (!validIds.Contains(id))
            {
                missingReferences.Add($"Profile '{profileId}' references missing {propertyName.TrimEnd('s')} id '{id}'");
            }
        }
    }
}

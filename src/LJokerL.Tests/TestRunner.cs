using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using LJokerL.Core.Models;
using LJokerL.Core.Services;

namespace LJokerL.Tests;

class Program
{
    static int Main(string[] args)
    {
        Console.WriteLine("=================================================");
        Console.WriteLine("LJokerL Control Center - Test Suite v2.0");
        Console.WriteLine("=================================================");
        Console.WriteLine();

        var runner = new TestRunner();
        return runner.RunAll();
    }
}

public class TestRunner
{
    private int _passed = 0;
    private int _failed = 0;

    public int RunAll()
    {
        RunModelTests();
        RunDetectorTests();
        RunConfigTests();
        RunProfilerTests();
        RunAutoConfigTests();
        RunBenchmarkSystemTests();
        
        Console.WriteLine();
        Console.WriteLine("=================================================");
        Console.WriteLine("TEST SUMMARY");
        Console.WriteLine("=================================================");
        Console.WriteLine($"Total:  {_passed + _failed}");
        Console.WriteLine($"Passed: {_passed}");
        Console.WriteLine($"Failed: {_failed}");
        Console.WriteLine();
        
        if (_failed > 0)
        {
            Console.WriteLine("SOME TESTS FAILED!");
            return 1;
        }
        
        Console.WriteLine("ALL TESTS PASSED!");
        return 0;
    }

    private void RunModelTests()
    {
        Console.WriteLine("MODEL TESTS:");
        
        Test("ExtendedHardwareSnapshot initialization", () =>
        {
            var snapshot = new ExtendedHardwareSnapshot();
            Assert(snapshot.Cpu != null);
            Assert(snapshot.Gpus != null);
            Assert(snapshot.Memory != null);
        });

        Test("CpuInfoExtended properties", () =>
        {
            var cpu = new CpuInfoExtended
            {
                Name = "Intel Core i9-13900K",
                PhysicalCores = 24,
                LogicalProcessors = 32,
                IsHybrid = true
            };
            Assert(cpu.Name == "Intel Core i9-13900K");
            Assert(cpu.PhysicalCores == 24);
            Assert(cpu.IsHybrid == true);
        });

        Test("GpuInfoExtended properties", () =>
        {
            var gpu = new GpuInfoExtended
            {
                Name = "NVIDIA RTX 4090",
                Vendor = "NVIDIA",
                VramBytes = 24UL * 1024 * 1024 * 1024
            };
            Assert(gpu.Vendor == "NVIDIA");
            Assert(gpu.VramBytes > 20UL * 1024 * 1024 * 1024);
        });

        Test("MemoryInfoExtended properties", () =>
        {
            var memory = new MemoryInfoExtended
            {
                TotalBytes = 32UL * 1024 * 1024 * 1024,
                SpeedMhz = 6000
            };
            Assert(memory.SpeedMhz == 6000);
            Assert(memory.TotalBytes > 30UL * 1024 * 1024 * 1024);
        });

        Test("StorageDriveInfo properties", () =>
        {
            var drive = new StorageDriveInfo
            {
                DriveLetter = "C",
                IsNvme = true,
                IsSsd = true
            };
            Assert(drive.IsNvme == true);
            Assert(drive.IsSsd == true);
        });

        Test("AutoConfigResult initialization", () =>
        {
            var result = new AutoConfigResult();
            Assert(result.Recommendations != null);
            Assert(result.Summary != null);
        });

        Console.WriteLine();
    }

    private void RunDetectorTests()
    {
        Console.WriteLine("DETECTOR TESTS:");

        Test("GpuDetector can be instantiated", () =>
        {
            var detector = new GpuDetector();
            Assert(detector != null);
        });

        Test("StorageDetector can be instantiated", () =>
        {
            var detector = new StorageDetector();
            Assert(detector != null);
        });

        Test("MonitorDetector can be instantiated", () =>
        {
            var detector = new MonitorDetector();
            Assert(detector != null);
        });

        Test("SoftwareDetector can be instantiated", () =>
        {
            var detector = new SoftwareDetector();
            Assert(detector != null);
        });

        Test("AutoConfigurationEngine can be instantiated", () =>
        {
            var engine = new AutoConfigurationEngine();
            var rules = engine.GetAllRules();
            if (rules == null)
                throw new Exception("AutoConfigurationEngine returned a null rules collection.");
            Assert(rules.Count > 0);
        });

        Console.WriteLine();
    }

    private void RunConfigTests()
    {
        Console.WriteLine("CONFIG LOADER TESTS:");

        Test("ConfigLoader can be instantiated", () =>
        {
            var loader = new ConfigLoader();
            Assert(loader != null);
        });

        Test("Profiles.json exists", () =>
        {
            var path = FindConfigPath("profiles.json");
            Assert(File.Exists(path));
        });

        Test("Tweaks.json exists", () =>
        {
            var path = FindConfigPath("tweaks.json");
            Assert(File.Exists(path));
        });

        Test("Hardware_rules.json exists", () =>
        {
            var path = FindConfigPath("hardware_rules.json");
            Assert(File.Exists(path));
        });

        Test("Config files are valid JSON", () =>
        {
            var profilesPath = FindConfigPath("profiles.json");
            var tweaksPath = FindConfigPath("tweaks.json");
            
            var profilesJson = File.ReadAllText(profilesPath);
            var tweaksJson = File.ReadAllText(tweaksPath);
            
            Assert(profilesJson.Contains("profiles"));
            Assert(tweaksJson.Contains("tweaks"));
        });

        Test("Profile references resolve to existing tweak, service, and app ids", () =>
        {
            var profilesPath = FindConfigPath("profiles.json");
            var tweaksPath = FindConfigPath("tweaks.json");
            var servicesPath = FindConfigPath("services.json");
            var appsPath = FindConfigPath("apps_bloatware.json");

            var tweakIds = LoadCatalogIds(tweaksPath, "tweaks");
            var serviceIds = LoadCatalogIds(servicesPath, "services");
            var appIds = LoadCatalogIds(appsPath, "apps");
            var missingReferences = new List<string>();

            using var document = JsonDocument.Parse(File.ReadAllText(profilesPath));
            foreach (var profile in document.RootElement.GetProperty("profiles").EnumerateArray())
            {
                var profileId = profile.GetProperty("id").GetString() ?? "<unknown-profile>";
                CollectMissingReferences(profile, "tweaks", tweakIds, missingReferences, profileId);
                CollectMissingReferences(profile, "services", serviceIds, missingReferences, profileId);
                CollectMissingReferences(profile, "apps", appIds, missingReferences, profileId);
            }

            Assert(missingReferences.Count == 0, string.Join("; ", missingReferences));
        });

        Test("SelectedProfileIdExistsInProfiles", () =>
        {
            var selectedProfilePath = FindConfigPath("selected_profile.txt");
            Assert(File.Exists(selectedProfilePath), "Config/selected_profile.txt was not found.");

            var selectedProfile = File.ReadAllText(selectedProfilePath).Trim();
            Assert(!string.IsNullOrWhiteSpace(selectedProfile), "Config/selected_profile.txt is empty.");

            var profileIds = LoadCatalogIds(FindConfigPath("profiles.json"), "profiles");
            Assert(profileIds.Contains(selectedProfile), $"Selected profile '{selectedProfile}' was not found in profiles.json.");
        });

        Console.WriteLine();
    }

    private void RunProfilerTests()
    {
        Console.WriteLine("SYSTEM PROFILER TESTS:");

        Test("SystemProfiler can be instantiated", () =>
        {
            var profiler = new SystemProfiler();
            Assert(profiler != null);
        });

        Test("HardwareDetector can be instantiated", () =>
        {
            var detector = new HardwareDetector();
            Assert(detector != null);
        });

        Console.WriteLine();
    }

    private void RunAutoConfigTests()
    {
        Console.WriteLine("AUTO CONFIGURATION TESTS:");

        Test("AutoConfigurationEngine has default rules", () =>
        {
            var engine = new AutoConfigurationEngine();
            var rules = engine.GetAllRules();
            Assert(rules.Count > 0);
        });

        Test("GenerateRecommendations with gaming hardware", () =>
        {
            var engine = new AutoConfigurationEngine();
            var hardware = CreateTestHardware("gaming");
            var result = engine.GenerateRecommendations(hardware);
            if (result == null)
                throw new Exception("GenerateRecommendations returned null for gaming hardware.");
            
            Assert(result.Recommendations.Count > 0);
            Assert(result.Summary.RecommendedProfile == "smooth_vrr");
        });

        Test("GenerateRecommendations with non-VRR gaming hardware", () =>
        {
            var engine = new AutoConfigurationEngine();
            var hardware = CreateTestHardware("gaming_no_vrr");
            var result = engine.GenerateRecommendations(hardware);
            if (result == null)
                throw new Exception("GenerateRecommendations returned null for non-VRR gaming hardware.");

            Assert(result.Summary.RecommendedProfile == "competitive_latency");
        });

        Test("GenerateRecommendations with laptop hardware", () =>
        {
            var engine = new AutoConfigurationEngine();
            var hardware = CreateTestHardware("laptop");
            var result = engine.GenerateRecommendations(hardware);
            if (result == null)
                throw new Exception("GenerateRecommendations returned null for laptop hardware.");
            
            Assert(result.Summary.RecommendedProfile == "laptop_saver");
        });

        Test("GenerateRecommendations with low RAM", () =>
        {
            var engine = new AutoConfigurationEngine();
            var hardware = CreateTestHardware("low_ram");
            var result = engine.GenerateRecommendations(hardware);
            if (result == null)
                throw new Exception("GenerateRecommendations returned null for low-RAM hardware.");
            
            Assert(result.Summary.RecommendedProfile == "low_resources");
        });

        Test("AutoConfiguration recommended profiles exist in profiles.json", () =>
        {
            var engine = new AutoConfigurationEngine();
            var profileIds = LoadCatalogIds(FindConfigPath("profiles.json"), "profiles");
            var hardwareScenarios = new[]
            {
                CreateTestHardware("gaming"),
                CreateTestHardware("gaming_no_vrr"),
                CreateTestHardware("laptop"),
                CreateTestHardware("low_ram")
            };

            foreach (var hardware in hardwareScenarios)
            {
                var result = engine.GenerateRecommendations(hardware);
                if (result == null)
                    throw new Exception("GenerateRecommendations returned null while validating profile ids.");

                var recommendedProfile = result.Summary?.RecommendedProfile;
                Assert(!string.IsNullOrWhiteSpace(recommendedProfile), "Recommended profile was empty.");
                Assert(profileIds.Contains(recommendedProfile!), $"Recommended profile '{recommendedProfile}' was not found in profiles.json.");
            }
        });

        Console.WriteLine();
    }

    private ExtendedHardwareSnapshot CreateTestHardware(string type)
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
            Gpus = new System.Collections.Generic.List<GpuInfoExtended>
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
            Monitors = new System.Collections.Generic.List<MonitorInfoExtended>
            {
                new MonitorInfoExtended
                {
                    VrrSupported = type != "gaming_no_vrr",
                    RefreshRateHz = type == "gaming" || type == "gaming_no_vrr" ? 240 : 144
                }
            },
            Storage = new System.Collections.Generic.List<StorageDriveInfo>
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
                GamingApps = new System.Collections.Generic.List<GamingAppInfo>
                {
                    new GamingAppInfo { Name = "Steam", IsInstalled = true }
                }
            }
        };

        return snapshot;
    }

    private string FindConfigPath(string filename)
    {
        var paths = new[]
        {
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

    private HashSet<string> LoadCatalogIds(string path, string rootProperty)
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

    private void CollectMissingReferences(JsonElement profile, string propertyName, HashSet<string> validIds, List<string> missingReferences, string profileId)
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

    private void Test(string name, Action test)
    {
        try
        {
            test();
            _passed++;
            Console.WriteLine($"  [PASS] {name}");
        }
        catch (Exception ex)
        {
            _failed++;
            Console.WriteLine($"  [FAIL] {name}: {ex.Message}");
        }
    }

    private void Assert(bool condition)
    {
        if (!condition)
            throw new Exception("Assertion failed");
    }

    private void Assert(bool condition, string message)
    {
        if (!condition)
            throw new Exception(message);
    }

    private void RunBenchmarkSystemTests()
    {
        Console.WriteLine("BENCHMARK SYSTEM TESTS:");

        Test("BenchmarkRunner measures CPU Jitter and memory pointer-chasing latency", () =>
        {
            var runner = new BenchmarkRunner();
            // Run a quick benchmark (no network/disk to keep test execution extremely fast)
            var options = BenchmarkRunOptions.CreateQuick();
            options.RunCpu = true;
            options.RunMemory = true;
            options.CpuPrimeLimit = 1000; // Small prime limit for fast tests
            options.MemoryMaxMb = 10;     // Small memory footprint
            
            var result = runner.Run(options);
            
            Assert(result != null);
            if (result == null || result.Cpu == null || result.Memory == null)
            {
                throw new Exception("Benchmark failed: CPU or Memory result is null.");
            }
            Assert(result.Cpu.ThreadSchedulingJitterMs > 0);
            Assert(result.Memory.LatencyPointerChasingNs > 0);
        });

        Test("BenchmarkComparer compares runs and calculates latency deltas", () =>
        {
            var before = new BenchmarkRunResult
            {
                OverallScore = 80.0,
                Cpu = new CpuBenchmarkResult { Score = 80, ThreadSchedulingJitterMs = 1.25 },
                Memory = new MemoryBenchmarkResult { Score = 85, LatencyPointerChasingNs = 55.4 },
                Network = new NetworkBenchmarkResult { Score = 90, OverallJitterMs = 4.2 }
            };
            
            var after = new BenchmarkRunResult
            {
                OverallScore = 90.0,
                Cpu = new CpuBenchmarkResult { Score = 92, ThreadSchedulingJitterMs = 0.85 },
                Memory = new MemoryBenchmarkResult { Score = 95, LatencyPointerChasingNs = 48.2 },
                Network = new NetworkBenchmarkResult { Score = 98, OverallJitterMs = 1.8 }
            };

            var comparison = BenchmarkComparer.Compare(before, after);
            
            Assert(comparison.OverallPercentChange.HasValue);
            Assert(comparison.CpuJitterDelta == Math.Round(0.85 - 1.25, 3));
            Assert(comparison.MemLatencyDelta == Math.Round(48.2 - 55.4, 2));
            Assert(comparison.NetJitterDelta == Math.Round(1.8 - 4.2, 2));
        });

        Test("BenchmarkReportGenerator outputs HTML and Markdown reports successfully", () =>
        {
            var before = new BenchmarkRunResult
            {
                OverallScore = 80.0,
                Cpu = new CpuBenchmarkResult { Score = 80, ThreadSchedulingJitterMs = 1.25, CPUName = "Intel Test Core" },
                Memory = new MemoryBenchmarkResult { Score = 85, LatencyPointerChasingNs = 55.4, TotalRAM = 16.0 },
                Network = new NetworkBenchmarkResult { Score = 90, OverallJitterMs = 4.2 },
                System = new SystemBenchmarkResult { OSName = "Windows 11 Test", OSVersion = "22H2", ComputerModel = "Test PC", BIOSVersion = "V1.0" }
            };
            
            var after = new BenchmarkRunResult
            {
                OverallScore = 90.0,
                Cpu = new CpuBenchmarkResult { Score = 92, ThreadSchedulingJitterMs = 0.85, CPUName = "Intel Test Core" },
                Memory = new MemoryBenchmarkResult { Score = 95, LatencyPointerChasingNs = 48.2, TotalRAM = 16.0 },
                Network = new NetworkBenchmarkResult { Score = 98, OverallJitterMs = 1.8 },
                System = new SystemBenchmarkResult { OSName = "Windows 11 Test", OSVersion = "22H2", ComputerModel = "Test PC", BIOSVersion = "V1.0" }
            };

            var tempDir = Path.Combine(AppContext.BaseDirectory, "Benchmarks", "TestReports");
            var (htmlPath, mdPath) = BenchmarkReportGenerator.GenerateReport(before, after, tempDir);
            
            Assert(File.Exists(htmlPath));
            Assert(File.Exists(mdPath));

            var htmlContent = File.ReadAllText(htmlPath);
            var mdContent = File.ReadAllText(mdPath);

            Assert(htmlContent.Contains("Intel Test Core"));
            Assert(htmlContent.Contains("55.40ns"));
            Assert(mdContent.Contains("Intel Test Core"));
            Assert(mdContent.Contains("55.40 ns"));

            // Clean up files
            try
            {
                File.Delete(htmlPath);
                File.Delete(mdPath);
                Directory.Delete(tempDir);
            }
            catch {}
        });

        Console.WriteLine();
    }
}

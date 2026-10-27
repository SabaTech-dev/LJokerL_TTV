using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using LJokerL.Core.Models;
using Microsoft.Win32;

namespace LJokerL.Core.Services;

public sealed class BenchmarkRunner
{
    private const string BenchmarkVersion = "12.5";

    public BenchmarkRunResult Run(BenchmarkRunOptions options, IProgress<BenchmarkProgress>? progress = null)
    {
        var startTime = DateTime.Now;
        var result = new BenchmarkRunResult();
        var testsTotal = CountTests(options);
        var testsCompleted = 0;

        void Report(string stage)
        {
            progress?.Report(new BenchmarkProgress
            {
                Stage = stage,
                Completed = testsCompleted,
                Total = testsTotal
            });
        }

        Report("starting");

        if (options.RunCpu)
        {
            Report("cpu");
            result.Cpu = RunCpuTest(options.CpuPrimeLimit);
            testsCompleted++;
        }

        if (options.RunMemory)
        {
            Report("memory");
            result.Memory = RunMemoryTest(options.MemoryMaxMb);
            testsCompleted++;
        }

        if (options.RunDisk)
        {
            Report("disk");
            result.Disk = RunDiskTest(options.DiskFileSizeMb, options.DiskBlockSizeKb);
            testsCompleted++;
        }

        if (options.RunNetwork)
        {
            Report("network");
            result.Network = RunNetworkTest(options.PingTargets, options.PingCount);
            testsCompleted++;
        }

        if (options.RunSystem)
        {
            Report("system");
            result.System = RunSystemTest();
            testsCompleted++;
        }

        var endTime = DateTime.Now;
        var duration = (endTime - startTime).TotalSeconds;

        result.BenchmarkInfo = new BenchmarkInfo
        {
            Version = BenchmarkVersion,
            TestType = options.TestType,
            StartTime = startTime.ToString("yyyy-MM-dd HH:mm:ss"),
            EndTime = endTime.ToString("yyyy-MM-dd HH:mm:ss"),
            Duration = Math.Round(duration, 2),
            TestsExecuted = testsCompleted
        };

        result.OverallScore = ComputeOverallScore(result);
        Report("done");

        return result;
    }

    private static int CountTests(BenchmarkRunOptions options)
    {
        var count = 0;
        if (options.RunCpu) count++;
        if (options.RunMemory) count++;
        if (options.RunDisk) count++;
        if (options.RunNetwork) count++;
        if (options.RunSystem) count++;
        return Math.Max(1, count);
    }

    private static CpuBenchmarkResult RunCpuTest(int primeLimit)
    {
        var cpuInfo = GetCpuInfo();
        var startTime = DateTime.Now;
        var sw = Stopwatch.StartNew();
        var primeCount = 0;

        for (var i = 2; i <= primeLimit; i++)
        {
            var isPrime = true;
            var limit = (int)Math.Sqrt(i);
            for (var j = 2; j <= limit; j++)
            {
                if (i % j == 0)
                {
                    isPrime = false;
                    break;
                }
            }

            if (isPrime)
            {
                primeCount++;
            }
        }

        sw.Stop();
        var duration = sw.Elapsed.TotalSeconds;
        var primesPerSecond = duration > 0 ? primeCount / duration : 0;
        var cpuUsage = SampleCpuUsage();
        var temperature = TryReadTemperature();

        // Measure thread sleep precision jitter (representing Windows scheduler precision / DPC latency)
        var jitterList = new List<double>();
        for (var idx = 0; idx < 50; idx++)
        {
            var sleepSw = Stopwatch.StartNew();
            System.Threading.Thread.Sleep(1);
            sleepSw.Stop();
            var elapsedMs = sleepSw.Elapsed.TotalMilliseconds;
            jitterList.Add(Math.Abs(elapsedMs - 1.0));
        }
        var threadSchedulingJitterMs = jitterList.Count > 0 ? Math.Round(jitterList.Average(), 3) : 0.0;

        return new CpuBenchmarkResult
        {
            CPUName = cpuInfo.Name,
            Cores = cpuInfo.Cores,
            Threads = cpuInfo.Threads,
            BaseFrequency = cpuInfo.BaseFrequency,
            PrimesCalculated = primeCount,
            TestDuration = Math.Round(duration, 2),
            PrimesPerSecond = Math.Round(primesPerSecond, 2),
            AverageCPUUsage = Math.Round(cpuUsage, 2),
            ThreadSchedulingJitterMs = threadSchedulingJitterMs,
            Temperature = temperature,
            Score = Math.Round(Math.Min(100, primesPerSecond / 10), 0),
            Timestamp = startTime.ToString("yyyy-MM-dd HH:mm:ss")
        };
    }

    private static MemoryBenchmarkResult RunMemoryTest(int maxMemoryMb)
    {
        var startTime = DateTime.Now;
        var status = GetMemoryStatus();
        var totalRam = Math.Round(status.TotalBytes / 1024d / 1024d / 1024d, 2);
        var availableRam = Math.Round(status.AvailableBytes / 1024d / 1024d / 1024d, 2);

        var availableMb = status.AvailableBytes / 1024d / 1024d;
        var maxMemory = Math.Min(maxMemoryMb, availableMb * 0.25);
        var elementSize = 4;
        var maxElements = (int)Math.Floor(maxMemory * 1024d * 1024d / elementSize);
        var arraySize = Math.Min(25_000_000, Math.Max(1_000_000, maxElements));

        var writeSw = Stopwatch.StartNew();
        var testArray = new int[arraySize];
        for (var i = 0; i < arraySize; i += 1000)
        {
            var endIndex = Math.Min(i + 999, arraySize - 1);
            for (var j = i; j <= endIndex; j++)
            {
                testArray[j] = j;
            }
        }
        writeSw.Stop();

        var readSw = Stopwatch.StartNew();
        long sum = 0;
        for (var i = 0; i < arraySize; i += 1000)
        {
            var endIndex = Math.Min(i + 999, arraySize - 1);
            for (var j = i; j <= endIndex; j++)
            {
                sum += testArray[j];
            }
        }
        readSw.Stop();
        GC.KeepAlive(sum);

        var randomSw = Stopwatch.StartNew();
        var random = new Random();
        var randomAccesses = Math.Min(100_000, arraySize / 10);
        for (var i = 0; i < randomAccesses; i++)
        {
            var index = random.Next(0, arraySize);
            _ = testArray[index];
        }
        randomSw.Stop();

        var latencySw = Stopwatch.StartNew();
        var latencyTests = 10_000;
        for (var i = 0; i < latencyTests; i++)
        {
            var index = random.Next(0, arraySize);
            testArray[index] = testArray[index] + 1;
        }
        latencySw.Stop();

        // Elite Pointer-Chasing Memory Latency (bypassing prefetchers with randomized 2MB array chain)
        int chasingSize = 512 * 1024; // 2MB total size
        var chasingArray = new int[chasingSize];
        var indices = new int[chasingSize];
        for (var i = 0; i < chasingSize; i++)
        {
            indices[i] = i;
        }

        var rng = new Random(42); // Seeded for consistency
        for (var i = chasingSize - 1; i > 0; i--)
        {
            var j = rng.Next(i + 1);
            var temp = indices[i];
            indices[i] = indices[j];
            indices[j] = temp;
        }

        for (var i = 0; i < chasingSize - 1; i++)
        {
            chasingArray[indices[i]] = indices[i + 1];
        }
        chasingArray[indices[chasingSize - 1]] = indices[0];

        // Warmup
        var curr = 0;
        for (var i = 0; i < 10000; i++)
        {
            curr = chasingArray[curr];
        }

        // Measure random latency
        var cycles = 1000000;
        var chasingSw = Stopwatch.StartNew();
        for (var i = 0; i < cycles; i++)
        {
            curr = chasingArray[curr];
        }
        chasingSw.Stop();
        GC.KeepAlive(curr);

        var latencyPointerChasingNs = Math.Round((chasingSw.Elapsed.TotalSeconds * 1e9) / cycles, 2);

        testArray = Array.Empty<int>();
        chasingArray = Array.Empty<int>();
        indices = Array.Empty<int>();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        var writeTime = writeSw.Elapsed.TotalSeconds;
        var readTime = readSw.Elapsed.TotalSeconds;
        var randomTime = randomSw.Elapsed.TotalSeconds;
        var latencyTime = latencySw.Elapsed.TotalSeconds;

        var bytesProcessed = arraySize * elementSize;
        var writeSpeed = writeTime > 0 ? bytesProcessed / writeTime / (1024d * 1024d) : 0;
        var readSpeed = readTime > 0 ? bytesProcessed / readTime / (1024d * 1024d) : 0;
        var randomSpeed = randomTime > 0 ? randomAccesses / (randomTime * 1000d) : 0;
        var avgLatencyMs = latencyTests > 0 ? (latencyTime * 1000d) / latencyTests : 0;

        var writeScore = Math.Min(100, (writeSpeed / 1000d) * 100d);
        var readScore = Math.Min(100, (readSpeed / 1000d) * 100d);
        var randomScore = Math.Min(100, (randomSpeed / 100d) * 100d);
        var latencyScore = Math.Max(0, 100 - (avgLatencyMs * 10));
        var finalScore = Math.Round((writeScore + readScore + randomScore + latencyScore) / 4d, 0);

        return new MemoryBenchmarkResult
        {
            TotalRAM = totalRam,
            AvailableRAM = availableRam,
            ArraySize = arraySize,
            ArraySizeMB = Math.Round(bytesProcessed / (1024d * 1024d), 2),
            WriteTime = Math.Round(writeTime, 3),
            ReadTime = Math.Round(readTime, 3),
            RandomAccessTime = Math.Round(randomTime, 3),
            LatencyTime = Math.Round(latencyTime, 3),
            WriteSpeedMB = Math.Round(writeSpeed, 2),
            ReadSpeedMB = Math.Round(readSpeed, 2),
            RandomSpeedK = Math.Round(randomSpeed, 2),
            AvgLatencyMs = Math.Round(avgLatencyMs, 3),
            LatencyPointerChasingNs = latencyPointerChasingNs,
            WriteScore = Math.Round(writeScore, 0),
            ReadScore = Math.Round(readScore, 0),
            RandomScore = Math.Round(randomScore, 0),
            LatencyScore = Math.Round(latencyScore, 0),
            Score = finalScore,
            Timestamp = startTime.ToString("yyyy-MM-dd HH:mm:ss")
        };
    }

    private static DiskBenchmarkResult RunDiskTest(int fileSizeMb, int blockSizeKb)
    {
        var startTime = DateTime.Now;
        var tempRoot = Path.Combine(AppContext.BaseDirectory, "Benchmarks", "Temp");
        Directory.CreateDirectory(tempRoot);

        var testFile = Path.Combine(tempRoot, "benchmark_disk_test.tmp");
        var fileSizeBytes = fileSizeMb * 1024L * 1024L;
        var blockSize = blockSizeKb * 1024;
        var data = new byte[blockSize];
        new Random().NextBytes(data);

        var writeSw = Stopwatch.StartNew();
        using (var stream = new FileStream(testFile, FileMode.Create, FileAccess.Write, FileShare.None, blockSize, FileOptions.SequentialScan))
        {
            var remaining = fileSizeBytes;
            while (remaining > 0)
            {
                var toWrite = (int)Math.Min(blockSize, remaining);
                stream.Write(data, 0, toWrite);
                remaining -= toWrite;
            }
        }
        writeSw.Stop();

        var readSw = Stopwatch.StartNew();
        using (var stream = new FileStream(testFile, FileMode.Open, FileAccess.Read, FileShare.Read, blockSize, FileOptions.SequentialScan))
        {
            var buffer = new byte[blockSize];
            while (stream.Read(buffer, 0, buffer.Length) > 0)
            {
            }
        }
        readSw.Stop();

        try
        {
            File.Delete(testFile);
        }
        catch
        {
        }

        var writeTime = writeSw.Elapsed.TotalSeconds;
        var readTime = readSw.Elapsed.TotalSeconds;
        var writeSpeed = writeTime > 0 ? fileSizeBytes / writeTime / (1024d * 1024d) : 0;
        var readSpeed = readTime > 0 ? fileSizeBytes / readTime / (1024d * 1024d) : 0;

        var driveInfo = GetPrimaryDriveInfo();

        return new DiskBenchmarkResult
        {
            DriveModel = driveInfo.Model,
            DriveSize = driveInfo.TotalGb,
            FreeSpace = driveInfo.FreeGb,
            FileSize = Math.Round(fileSizeBytes / 1024d / 1024d, 2),
            WriteTime = Math.Round(writeTime, 3),
            ReadTime = Math.Round(readTime, 3),
            WriteSpeed = Math.Round(writeSpeed, 2),
            ReadSpeed = Math.Round(readSpeed, 2),
            Score = Math.Min(100, Math.Round((writeSpeed + readSpeed) / 50d, 0)),
            Timestamp = startTime.ToString("yyyy-MM-dd HH:mm:ss")
        };
    }

    private static NetworkBenchmarkResult RunNetworkTest(IReadOnlyList<string> targets, int pingCount)
    {
        var startTime = DateTime.Now;
        var results = new List<NetworkPingResult>();
        var adapterCount = NetworkInterface.GetAllNetworkInterfaces().Count(nic =>
            nic.OperationalStatus == OperationalStatus.Up &&
            nic.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
            nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel);

        foreach (var target in targets)
        {
            var latencies = new List<long>();
            var failures = 0;
            using var ping = new Ping();
            for (var i = 0; i < pingCount; i++)
            {
                try
                {
                    var reply = ping.Send(target, 1000);
                    if (reply != null && reply.Status == IPStatus.Success)
                    {
                        latencies.Add(reply.RoundtripTime);
                    }
                    else
                    {
                        failures++;
                    }
                }
                catch
                {
                    failures++;
                }
            }

            if (latencies.Count > 0)
            {
                var avg = latencies.Average();
                double jitter = 0;
                if (latencies.Count > 1)
                {
                    double sumOfSquares = latencies.Sum(val => (val - avg) * (val - avg));
                    jitter = Math.Sqrt(sumOfSquares / (latencies.Count - 1));
                }

                results.Add(new NetworkPingResult
                {
                    Target = target,
                    AvgLatency = Math.Round(avg, 2),
                    MinLatency = latencies.Min(),
                    MaxLatency = latencies.Max(),
                    PacketLoss = Math.Round(failures * 100d / pingCount, 2),
                    JitterMs = Math.Round(jitter, 2)
                });
            }
        }

        var overallLatency = results.Count > 0 ? results.Average(r => r.AvgLatency) : 0;
        var overallJitter = results.Count > 0 ? results.Average(r => r.JitterMs) : 0;
        var score = results.Count > 0 ? Math.Max(0, Math.Round(100 - overallLatency, 0)) : 0;

        return new NetworkBenchmarkResult
        {
            NetworkAdapters = adapterCount,
            PingResults = results,
            OverallLatency = Math.Round(overallLatency, 2),
            OverallJitterMs = Math.Round(overallJitter, 2),
            Score = score,
            Timestamp = startTime.ToString("yyyy-MM-dd HH:mm:ss")
        };
    }

    private static SystemBenchmarkResult RunSystemTest()
    {
        var startTime = DateTime.Now;
        var osInfo = GetOsInfo();
        var cpuUsage = SampleCpuUsage();
        var memoryStatus = GetMemoryStatus();
        var memoryUsage = memoryStatus.TotalBytes == 0
            ? 0
            : (1 - (memoryStatus.AvailableBytes / (double)memoryStatus.TotalBytes)) * 100;

        var processes = 0;
        try
        {
            processes = Process.GetProcesses().Length;
        }
        catch
        {
        }

        var score = Math.Round(100 - (cpuUsage + memoryUsage) / 2d, 0);

        return new SystemBenchmarkResult
        {
            OSName = osInfo.OSName,
            OSVersion = osInfo.OSVersion,
            ComputerModel = osInfo.ComputerModel,
            BIOSVersion = osInfo.BiosVersion,
            LastBootTime = osInfo.LastBootTime,
            Uptime = osInfo.Uptime,
            RunningServices = osInfo.RunningServices,
            RunningProcesses = processes,
            CurrentCPUUsage = Math.Round(cpuUsage, 2),
            CurrentMemoryUsage = Math.Round(memoryUsage, 2),
            Score = score,
            Timestamp = startTime.ToString("yyyy-MM-dd HH:mm:ss")
        };
    }

    private static double ComputeOverallScore(BenchmarkRunResult result)
    {
        var scores = new List<double>();
        if (result.Cpu != null) scores.Add(result.Cpu.Score);
        if (result.Memory != null) scores.Add(result.Memory.Score);
        if (result.Disk != null) scores.Add(result.Disk.Score);
        if (result.Network != null) scores.Add(result.Network.Score);
        if (result.System != null) scores.Add(result.System.Score);

        return scores.Count == 0 ? 0 : Math.Round(scores.Average(), 2);
    }

    private static (string? Name, int? Cores, int? Threads, int? BaseFrequency) GetCpuInfo()
    {
        try
        {
            using var cpuKey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
            if (cpuKey != null)
            {
                var name = cpuKey.GetValue("ProcessorNameString")?.ToString();
                var mhz = cpuKey.GetValue("~MHz");
                var baseFrequency = mhz != null ? Convert.ToInt32(mhz) : (int?)null;
                return (name, null, Environment.ProcessorCount, baseFrequency);
            }
        }
        catch
        {
        }

        return (null, null, Environment.ProcessorCount, null);
    }

    private static double SampleCpuUsage()
    {
        if (!GetSystemTimes(out var idle1, out var kernel1, out var user1))
        {
            return 0;
        }

        System.Threading.Thread.Sleep(200);

        if (!GetSystemTimes(out var idle2, out var kernel2, out var user2))
        {
            return 0;
        }

        var idleDelta = idle2.ToUInt64() - idle1.ToUInt64();
        var totalDelta = (kernel2.ToUInt64() + user2.ToUInt64()) - (kernel1.ToUInt64() + user1.ToUInt64());
        if (totalDelta == 0)
        {
            return 0;
        }

        var usage = (totalDelta - idleDelta) * 100.0 / totalDelta;
        return Math.Clamp(usage, 0, 100);
    }

    private static string? TryReadTemperature()
    {
        return "N/A";
    }

    private static (string Model, double TotalGb, double FreeGb) GetPrimaryDriveInfo()
    {
        var totalGb = 0d;
        var freeGb = 0d;
        var model = "";

        try
        {
            var drive = DriveInfo.GetDrives().FirstOrDefault(d => d.DriveType == DriveType.Fixed && d.IsReady);
            if (drive != null)
            {
                totalGb = Math.Round(drive.TotalSize / 1024d / 1024d / 1024d, 2);
                freeGb = Math.Round(drive.AvailableFreeSpace / 1024d / 1024d / 1024d, 2);
                model = drive.Name.TrimEnd('\\');
            }
        }
        catch
        {
        }

        model = string.IsNullOrWhiteSpace(model) ? "" : model;

        return (model, totalGb, freeGb);
    }

    private static (string OSName, string OSVersion, string? ComputerModel, string? BiosVersion, string LastBootTime, string Uptime, int RunningServices) GetOsInfo()
    {
        var osName = RuntimeInformation.OSDescription;
        var osVersion = Environment.OSVersion.Version.ToString();
        var model = string.Empty;
        var bios = string.Empty;

        var uptimeSpan = TimeSpan.FromMilliseconds(Environment.TickCount64);
        var lastBoot = DateTime.Now.Subtract(uptimeSpan).ToString("yyyy-MM-dd HH:mm:ss");
        var uptime = $"{uptimeSpan.Days}d {uptimeSpan.Hours}h {uptimeSpan.Minutes}m";

        try
        {
            using var biosKey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\BIOS");
            if (biosKey != null)
            {
                model = biosKey.GetValue("SystemProductName")?.ToString() ?? string.Empty;
                var biosValue = biosKey.GetValue("BIOSVersion");
                if (biosValue is string[] biosLines)
                {
                    bios = string.Join(" ", biosLines);
                }
                else
                {
                    bios = biosValue?.ToString() ?? string.Empty;
                }
            }
        }
        catch
        {
        }

        return (osName, osVersion, string.IsNullOrWhiteSpace(model) ? null : model, string.IsNullOrWhiteSpace(bios) ? null : bios, lastBoot, uptime, 0);
    }

    private static MemoryStatus GetMemoryStatus()
    {
        var status = new MemoryStatus { Native = new MemoryStatusEx() };
        if (!GlobalMemoryStatusEx(ref status.Native))
        {
            return status;
        }

        status.TotalBytes = status.Native.ullTotalPhys;
        status.AvailableBytes = status.Native.ullAvailPhys;
        return status;
    }

    private sealed class MemoryStatus
    {
        public MemoryStatusEx Native;
        public ulong TotalBytes;
        public ulong AvailableBytes;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct MemoryStatusEx
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;

        public MemoryStatusEx()
        {
            dwLength = (uint)Marshal.SizeOf(typeof(MemoryStatusEx));
            dwMemoryLoad = 0;
            ullTotalPhys = 0;
            ullAvailPhys = 0;
            ullTotalPageFile = 0;
            ullAvailPageFile = 0;
            ullTotalVirtual = 0;
            ullAvailVirtual = 0;
            ullAvailExtendedVirtual = 0;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct FileTime
    {
        public uint dwLowDateTime;
        public uint dwHighDateTime;

        public ulong ToUInt64()
        {
            return ((ulong)dwHighDateTime << 32) | dwLowDateTime;
        }
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool GlobalMemoryStatusEx(ref MemoryStatusEx lpBuffer);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetSystemTimes(out FileTime idleTime, out FileTime kernelTime, out FileTime userTime);
}

using LJokerL.Core.Models;
using Xunit;

namespace LJokerL.Tests;

public class ModelTests
{
    [Fact]
    public void ExtendedHardwareSnapshot_initialization()
    {
        var snapshot = new ExtendedHardwareSnapshot();
        Assert.NotNull(snapshot.Cpu);
        Assert.NotNull(snapshot.Gpus);
        Assert.NotNull(snapshot.Memory);
    }

    [Fact]
    public void CpuInfoExtended_properties()
    {
        var cpu = new CpuInfoExtended
        {
            Name = "Intel Core i9-13900K",
            PhysicalCores = 24,
            LogicalProcessors = 32,
            IsHybrid = true
        };
        Assert.Equal("Intel Core i9-13900K", cpu.Name);
        Assert.Equal(24, cpu.PhysicalCores);
        Assert.True(cpu.IsHybrid);
    }

    [Fact]
    public void GpuInfoExtended_properties()
    {
        var gpu = new GpuInfoExtended
        {
            Name = "NVIDIA RTX 4090",
            Vendor = "NVIDIA",
            VramBytes = 24UL * 1024 * 1024 * 1024
        };
        Assert.Equal("NVIDIA", gpu.Vendor);
        Assert.True(gpu.VramBytes > 20UL * 1024 * 1024 * 1024, $"Expected VramBytes above 20 GiB but was {gpu.VramBytes}.");
    }

    [Fact]
    public void MemoryInfoExtended_properties()
    {
        var memory = new MemoryInfoExtended
        {
            TotalBytes = 32UL * 1024 * 1024 * 1024,
            SpeedMhz = 6000
        };
        Assert.Equal(6000, memory.SpeedMhz);
        Assert.True(memory.TotalBytes > 30UL * 1024 * 1024 * 1024, $"Expected TotalBytes above 30 GiB but was {memory.TotalBytes}.");
    }

    [Fact]
    public void StorageDriveInfo_properties()
    {
        var drive = new StorageDriveInfo
        {
            DriveLetter = "C",
            IsNvme = true,
            IsSsd = true
        };
        Assert.True(drive.IsNvme);
        Assert.True(drive.IsSsd);
    }

    [Fact]
    public void AutoConfigResult_initialization()
    {
        var result = new AutoConfigResult();
        Assert.NotNull(result.Recommendations);
        Assert.NotNull(result.Summary);
    }
}

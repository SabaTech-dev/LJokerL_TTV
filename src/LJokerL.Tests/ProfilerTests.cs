using LJokerL.Core.Services;
using Xunit;

namespace LJokerL.Tests;

public class ProfilerTests
{
    [Fact]
    public void SystemProfiler_can_be_instantiated()
    {
        var profiler = new SystemProfiler();
        Assert.NotNull(profiler);
    }

    [Fact]
    public void HardwareDetector_can_be_instantiated()
    {
        var detector = new HardwareDetector();
        Assert.NotNull(detector);
    }
}

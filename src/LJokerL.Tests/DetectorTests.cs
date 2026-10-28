using LJokerL.Core.Services;
using Xunit;

namespace LJokerL.Tests;

public class DetectorTests
{
    [Fact]
    public void GpuDetector_can_be_instantiated()
    {
        var detector = new GpuDetector();
        Assert.NotNull(detector);
    }

    [Fact]
    public void StorageDetector_can_be_instantiated()
    {
        var detector = new StorageDetector();
        Assert.NotNull(detector);
    }

    [Fact]
    public void MonitorDetector_can_be_instantiated()
    {
        var detector = new MonitorDetector();
        Assert.NotNull(detector);
    }

    [Fact]
    public void SoftwareDetector_can_be_instantiated()
    {
        var detector = new SoftwareDetector();
        Assert.NotNull(detector);
    }

    [Fact]
    public void AutoConfigurationEngine_can_be_instantiated()
    {
        var engine = new AutoConfigurationEngine();
        var rules = engine.GetAllRules();
        Assert.True(rules != null, "AutoConfigurationEngine returned a null rules collection.");
        Assert.True(rules.Count > 0, "AutoConfigurationEngine returned an empty rules collection.");
    }
}

using LJokerL.Core.Services;
using Xunit;

namespace LJokerL.Tests;

public class OsVersionCheckTests
{
    [Theory]
    [InlineData(22000, WindowsBuildSupport.Supported)]
    [InlineData(22631, WindowsBuildSupport.Supported)]
    [InlineData(26100, WindowsBuildSupport.Supported)]
    [InlineData(19041, WindowsBuildSupport.SupportedLegacy)]
    [InlineData(19045, WindowsBuildSupport.SupportedLegacy)]
    [InlineData(20348, WindowsBuildSupport.SupportedLegacy)]
    [InlineData(21999, WindowsBuildSupport.SupportedLegacy)]
    [InlineData(17763, WindowsBuildSupport.Unsupported)]
    [InlineData(14393, WindowsBuildSupport.Unsupported)]
    [InlineData(0, WindowsBuildSupport.Unsupported)]
    [InlineData(18999, WindowsBuildSupport.Unsupported)]
    public void Decide_classifies_windows_builds(int buildNumber, WindowsBuildSupport expected)
    {
        Assert.Equal(expected, OsVersionCheck.Decide(buildNumber));
    }
}

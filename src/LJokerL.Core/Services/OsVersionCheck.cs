using System;

namespace LJokerL.Core.Services;

public enum WindowsBuildSupport
{
    Supported,
    SupportedLegacy,
    Unsupported
}

public static class OsVersionCheck
{
    public static WindowsBuildSupport Decide(int buildNumber)
    {
        if (buildNumber >= 22000)
        {
            return WindowsBuildSupport.Supported;
        }

        if (buildNumber >= 19041)
        {
            return WindowsBuildSupport.SupportedLegacy;
        }

        return WindowsBuildSupport.Unsupported;
    }

    public static int CurrentBuild()
    {
        return Environment.OSVersion.Version.Build;
    }
}

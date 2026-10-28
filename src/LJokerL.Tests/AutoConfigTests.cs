using LJokerL.Core.Services;
using Xunit;

namespace LJokerL.Tests;

public class AutoConfigTests
{
    [Fact]
    public void AutoConfigurationEngine_has_default_rules()
    {
        var engine = new AutoConfigurationEngine();
        var rules = engine.GetAllRules();
        Assert.True(rules.Count > 0, "AutoConfigurationEngine returned an empty rules collection.");
    }

    [Fact]
    public void GenerateRecommendations_with_gaming_hardware()
    {
        var engine = new AutoConfigurationEngine();
        var hardware = TestHelpers.CreateTestHardware("gaming");
        var result = engine.GenerateRecommendations(hardware);
        Assert.True(result != null, "GenerateRecommendations returned null for gaming hardware.");

        Assert.True(result.Recommendations.Count > 0, "GenerateRecommendations returned no recommendations for gaming hardware.");
        Assert.Equal("smooth_vrr", result.Summary.RecommendedProfile);
    }

    [Fact]
    public void GenerateRecommendations_with_non_VRR_gaming_hardware()
    {
        var engine = new AutoConfigurationEngine();
        var hardware = TestHelpers.CreateTestHardware("gaming_no_vrr");
        var result = engine.GenerateRecommendations(hardware);
        Assert.True(result != null, "GenerateRecommendations returned null for non-VRR gaming hardware.");

        Assert.Equal("competitive_latency", result.Summary.RecommendedProfile);
    }

    [Fact]
    public void GenerateRecommendations_with_laptop_hardware()
    {
        var engine = new AutoConfigurationEngine();
        var hardware = TestHelpers.CreateTestHardware("laptop");
        var result = engine.GenerateRecommendations(hardware);
        Assert.True(result != null, "GenerateRecommendations returned null for laptop hardware.");

        Assert.Equal("laptop_saver", result.Summary.RecommendedProfile);
    }

    [Fact]
    public void GenerateRecommendations_with_low_RAM()
    {
        var engine = new AutoConfigurationEngine();
        var hardware = TestHelpers.CreateTestHardware("low_ram");
        var result = engine.GenerateRecommendations(hardware);
        Assert.True(result != null, "GenerateRecommendations returned null for low-RAM hardware.");

        Assert.Equal("low_resources", result.Summary.RecommendedProfile);
    }

    [Fact]
    public void AutoConfiguration_recommended_profiles_exist_in_profiles_json()
    {
        var engine = new AutoConfigurationEngine();
        var profileIds = TestHelpers.LoadCatalogIds(TestHelpers.FindConfigPath("profiles.json"), "profiles");
        var hardwareScenarios = new[]
        {
            TestHelpers.CreateTestHardware("gaming"),
            TestHelpers.CreateTestHardware("gaming_no_vrr"),
            TestHelpers.CreateTestHardware("laptop"),
            TestHelpers.CreateTestHardware("low_ram")
        };

        foreach (var hardware in hardwareScenarios)
        {
            var result = engine.GenerateRecommendations(hardware);
            Assert.True(result != null, "GenerateRecommendations returned null while validating profile ids.");

            var recommendedProfile = result.Summary?.RecommendedProfile;
            Assert.True(!string.IsNullOrWhiteSpace(recommendedProfile), "Recommended profile was empty.");
            Assert.True(profileIds.Contains(recommendedProfile!), $"Recommended profile '{recommendedProfile}' was not found in profiles.json.");
        }
    }
}

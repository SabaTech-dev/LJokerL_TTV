using System.Text.Json;
using LJokerL.Core.Services;
using Xunit;

namespace LJokerL.Tests;

public class ConfigTests
{
    [Fact]
    public void ConfigLoader_can_be_instantiated()
    {
        var loader = new ConfigLoader();
        Assert.NotNull(loader);
    }

    [Fact]
    public void Profiles_json_exists()
    {
        var path = TestHelpers.FindConfigPath("profiles.json");
        Assert.True(File.Exists(path), $"profiles.json was not found at '{path}'.");
    }

    [Fact]
    public void Tweaks_json_exists()
    {
        var path = TestHelpers.FindConfigPath("tweaks.json");
        Assert.True(File.Exists(path), $"tweaks.json was not found at '{path}'.");
    }

    [Fact]
    public void Hardware_rules_json_exists()
    {
        var path = TestHelpers.FindConfigPath("hardware_rules.json");
        Assert.True(File.Exists(path), $"hardware_rules.json was not found at '{path}'.");
    }

    [Fact]
    public void Config_files_are_valid_JSON()
    {
        var profilesPath = TestHelpers.FindConfigPath("profiles.json");
        var tweaksPath = TestHelpers.FindConfigPath("tweaks.json");

        var profilesJson = File.ReadAllText(profilesPath);
        var tweaksJson = File.ReadAllText(tweaksPath);

        Assert.True(profilesJson.Contains("profiles"), "profiles.json does not contain a 'profiles' section.");
        Assert.True(tweaksJson.Contains("tweaks"), "tweaks.json does not contain a 'tweaks' section.");
    }

    [Fact]
    public void Profile_references_resolve_to_existing_tweak_service_and_app_ids()
    {
        var profilesPath = TestHelpers.FindConfigPath("profiles.json");
        var tweaksPath = TestHelpers.FindConfigPath("tweaks.json");
        var servicesPath = TestHelpers.FindConfigPath("services.json");
        var appsPath = TestHelpers.FindConfigPath("apps_bloatware.json");

        var tweakIds = TestHelpers.LoadCatalogIds(tweaksPath, "tweaks");
        var serviceIds = TestHelpers.LoadCatalogIds(servicesPath, "services");
        var appIds = TestHelpers.LoadCatalogIds(appsPath, "apps");
        var missingReferences = new List<string>();

        using var document = JsonDocument.Parse(File.ReadAllText(profilesPath));
        foreach (var profile in document.RootElement.GetProperty("profiles").EnumerateArray())
        {
            var profileId = profile.GetProperty("id").GetString() ?? "<unknown-profile>";
            TestHelpers.CollectMissingReferences(profile, "tweaks", tweakIds, missingReferences, profileId);
            TestHelpers.CollectMissingReferences(profile, "services", serviceIds, missingReferences, profileId);
            TestHelpers.CollectMissingReferences(profile, "apps", appIds, missingReferences, profileId);
        }

        Assert.True(missingReferences.Count == 0, string.Join("; ", missingReferences));
    }
}

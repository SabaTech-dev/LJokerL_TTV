using System.Text.Json;
using LJokerL.Core.Models;
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
    public void Selected_profile_id_exists_in_profiles()
    {
        var selectedProfilePath = TestHelpers.FindConfigPath("selected_profile.txt");
        var id = File.ReadAllText(selectedProfilePath).Trim();
        Assert.False(string.IsNullOrWhiteSpace(id), "selected_profile.txt is empty or missing.");

        var profilesPath = TestHelpers.FindConfigPath("profiles.json");
        var ids = TestHelpers.LoadCatalogIds(profilesPath, "profiles");

        Assert.True(ids.Contains(id), $"Selected profile '{id}' was not found in profiles.json");
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

    [Fact]
    public void Game_watcher_catalog_is_valid_and_profiles_resolve()
    {
        var watcherPath = TestHelpers.FindConfigPath("game_watcher.json");
        Assert.True(File.Exists(watcherPath), $"game_watcher.json was not found at '{watcherPath}'.");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };
        var catalog = JsonSerializer.Deserialize<GameWatcherCatalog>(File.ReadAllText(watcherPath), options);
        Assert.NotNull(catalog);

        using var watcherDocument = JsonDocument.Parse(File.ReadAllText(watcherPath));
        var rawMappings = watcherDocument.RootElement.TryGetProperty("mappings", out var mappingsElement)
            && mappingsElement.ValueKind == JsonValueKind.Array
                ? mappingsElement.EnumerateArray().ToList()
                : new List<JsonElement>();

        Assert.Equal(rawMappings.Count, catalog!.Mappings.Count);

        var profilesPath = TestHelpers.FindConfigPath("profiles.json");
        var profileIds = TestHelpers.LoadCatalogIds(profilesPath, "profiles");

        for (var index = 0; index < catalog.Mappings.Count; index++)
        {
            var mapping = catalog.Mappings[index];
            Assert.False(string.IsNullOrWhiteSpace(mapping.ProcessName),
                $"Mapping at index {index} has an empty processName.");
            Assert.False(string.IsNullOrWhiteSpace(mapping.ProfileId),
                $"Mapping '{mapping.ProcessName}' has an empty profileId.");

            var omitted = !rawMappings[index].TryGetProperty("enabled", out _);
            if (omitted)
            {
                Assert.True(mapping.Enabled,
                    $"Mapping '{mapping.ProcessName}' omits 'enabled' but did not default to true.");
            }

            Assert.True(profileIds.Contains(mapping.ProfileId),
                $"Mapping '{mapping.ProcessName}' references profile '{mapping.ProfileId}' which does not exist in profiles.json.");
        }
    }

    [Fact]
    public void Affects_labels_only_on_matching_tweaks()
    {
        var path = TestHelpers.FindConfigPath("tweaks.json");
        var loader = new ConfigLoader();
        var catalog = loader.Load<TweakCatalog>(path);

        Assert.NotEmpty(catalog.Tweaks);

        var labeled = catalog.Tweaks
            .Where(t => t.AffectsDefender == true || t.AffectsUpdate == true)
            .ToList();

        Assert.True(labeled.Count > 0, "No tweak is labeled with affectsDefender/affectsUpdate; catalog labels missing.");

        foreach (var tweak in labeled)
        {
            var text = $"{tweak.Id} {tweak.Description}";
            var matches = text.Contains("defender", StringComparison.OrdinalIgnoreCase)
                || text.Contains("update", StringComparison.OrdinalIgnoreCase);
            Assert.True(matches,
                $"Tweak '{tweak.Id}' is labeled affectsDefender/affectsUpdate but its id/description does not mention Defender or Update.");
        }

        Assert.Contains(catalog.Tweaks, t => t.AffectsUpdate == true);
        Assert.Contains(catalog.Tweaks, t => t.AffectsDefender == null && t.AffectsUpdate == null);
    }
}

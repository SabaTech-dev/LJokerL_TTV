using System;
using System.IO;
using System.Text.Json;
using LJokerL.Core.Models;

namespace LJokerL.Core.Services;

public sealed class ConfigLoader
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip
    };

    public T Load<T>(string path) where T : class
    {
        if (TryLoad(path, out T? result, out var error))
        {
            return result!;
        }

        throw new InvalidOperationException(error ?? $"Failed to load config: {path}");
    }

    public ConfigBundle LoadAll(string configRoot)
    {
        var errors = new List<string>();

        var tweaks = TryLoad(Path.Combine(configRoot, "tweaks.json"), out TweakCatalog? tweakCatalog, out var tweakError)
            ? tweakCatalog
            : new TweakCatalog();
        if (tweakError != null)
        {
            errors.Add(tweakError);
        }

        var profiles = TryLoad(Path.Combine(configRoot, "profiles.json"), out ProfileCatalog? profileCatalog, out var profileError)
            ? profileCatalog
            : new ProfileCatalog();
        if (profileError != null)
        {
            errors.Add(profileError);
        }

        var services = TryLoad(Path.Combine(configRoot, "services.json"), out ServiceCatalog? serviceCatalog, out var serviceError)
            ? serviceCatalog
            : new ServiceCatalog();
        if (serviceError != null)
        {
            errors.Add(serviceError);
        }

        var apps = TryLoad(Path.Combine(configRoot, "apps_bloatware.json"), out AppCatalog? appCatalog, out var appError)
            ? appCatalog
            : new AppCatalog();
        if (appError != null)
        {
            errors.Add(appError);
        }

        var hardwareRules = TryLoad(Path.Combine(configRoot, "hardware_rules.json"), out HardwareRuleCatalog? ruleCatalog, out var ruleError)
            ? ruleCatalog
            : new HardwareRuleCatalog();
        if (ruleError != null)
        {
            errors.Add(ruleError);
        }

        return new ConfigBundle
        {
            Tweaks = tweaks,
            Profiles = profiles,
            Services = services,
            Apps = apps,
            HardwareRules = hardwareRules,
            Errors = errors
        };
    }

    private bool TryLoad<T>(string path, out T? result, out string? error) where T : class
    {
        result = null;
        error = null;

        if (!File.Exists(path))
        {
            error = $"Config file not found: {path}";
            return false;
        }

        try
        {
            var json = File.ReadAllText(path);
            result = JsonSerializer.Deserialize<T>(json, _options);
            if (result == null)
            {
                error = $"Failed to parse config: {path}";
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            error = $"Config load error ({path}): {ex.Message}";
            return false;
        }
    }
}

public sealed class ConfigBundle
{
    public TweakCatalog? Tweaks { get; set; }
    public ProfileCatalog? Profiles { get; set; }
    public ServiceCatalog? Services { get; set; }
    public AppCatalog? Apps { get; set; }
    public HardwareRuleCatalog? HardwareRules { get; set; }
    public List<string> Errors { get; set; } = new();
}

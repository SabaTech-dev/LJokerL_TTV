using System;
using System.IO;
using System.Text.Json;
using LJokerL.Core.Models;

namespace LJokerL.Core.Services;

public sealed class UserSettingsService
{
    private readonly string _settingsPath;
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public UserSettingsService(string settingsPath)
    {
        _settingsPath = settingsPath;
    }

    public UserSettings Load()
    {
        try
        {
            if (!File.Exists(_settingsPath))
            {
                return new UserSettings();
            }

            var json = File.ReadAllText(_settingsPath);
            var settings = JsonSerializer.Deserialize<UserSettings>(json, _options);
            return settings ?? new UserSettings();
        }
        catch
        {
            return new UserSettings();
        }
    }

    public void Save(UserSettings settings)
    {
        try
        {
            var directory = Path.GetDirectoryName(_settingsPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(settings, _options);
            File.WriteAllText(_settingsPath, json);
        }
        catch
        {
        }
    }
}

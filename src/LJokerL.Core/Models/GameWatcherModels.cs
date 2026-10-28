namespace LJokerL.Core.Models;

public sealed class GameWatcherMapping
{
    public string ProcessName { get; set; } = string.Empty;
    public string ProfileId { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
}

public sealed class GameWatcherCatalog
{
    public string Version { get; set; } = string.Empty;
    public List<GameWatcherMapping> Mappings { get; set; } = new();
}

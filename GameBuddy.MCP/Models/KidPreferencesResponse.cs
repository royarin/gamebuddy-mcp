using System.Text.Json.Serialization;

namespace GameBuddy.MCP.Models;

/// <summary>
/// Response containing kid's preferences.
/// </summary>
public class KidPreferencesResponse
{
    [JsonPropertyName("nickname")]
    public string Nickname { get; set; } = string.Empty;

    [JsonPropertyName("preferredDifficulty")]
    public string PreferredDifficulty { get; set; } = string.Empty;

    [JsonPropertyName("preferredQuizLength")]
    public int PreferredQuizLength { get; set; }

    [JsonPropertyName("topicsOfInterest")]
    public List<string> TopicsOfInterest { get; set; } = new();
}

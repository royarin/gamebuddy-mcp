using System.Text.Json.Serialization;

namespace GameBuddy.Api.Models;

/// <summary>
/// Data transfer object for kid preferences.
/// </summary>
public class KidPreferencesDto
{
    [JsonPropertyName("preferredDifficulty")]
    public string PreferredDifficulty { get; set; } = string.Empty;
    
    [JsonPropertyName("preferredQuizLength")]
    public int PreferredQuizLength { get; set; }
    
    [JsonPropertyName("topicsOfInterest")]
    public List<string> TopicsOfInterest { get; set; } = new();
}

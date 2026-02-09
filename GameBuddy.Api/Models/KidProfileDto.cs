using System.Text.Json.Serialization;

namespace GameBuddy.Api.Models;

/// <summary>
/// Data transfer object for kid profile information.
/// </summary>
public class KidProfileDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("age")]
    public int Age { get; set; }
}

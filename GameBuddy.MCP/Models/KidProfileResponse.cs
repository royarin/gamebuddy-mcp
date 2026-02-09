using System.Text.Json.Serialization;

namespace GameBuddy.MCP.Models;

/// <summary>
/// Response containing kid's basic profile information.
/// </summary>
public class KidProfileResponse
{
    [JsonPropertyName("nickname")]
    public string Nickname { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("age")]
    public int Age { get; set; }
}

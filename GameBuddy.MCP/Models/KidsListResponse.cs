using System.Text.Json.Serialization;

namespace GameBuddy.MCP.Models;

/// <summary>
/// Response containing list of all kids.
/// </summary>
public class KidsListResponse
{
    [JsonPropertyName("kids")]
    public List<string> Kids { get; set; } = new();
}

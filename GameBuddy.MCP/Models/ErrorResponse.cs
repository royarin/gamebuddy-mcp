using System.Text.Json.Serialization;

namespace GameBuddy.MCP.Models;

/// <summary>
/// Response containing error information.
/// </summary>
public class ErrorResponse
{
    [JsonPropertyName("error")]
    public string Error { get; set; } = string.Empty;

    public ErrorResponse(string error)
    {
        Error = error;
    }
}

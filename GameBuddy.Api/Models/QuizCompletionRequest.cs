using System.Text.Json.Serialization;

namespace GameBuddy.Api.Models;

/// <summary>
/// Request object for recording quiz completion and progress.
/// </summary>
public class QuizCompletionRequest
{
    [JsonPropertyName("countriesCovered")]
    public List<string> CountriesCovered { get; set; } = new();

    [JsonPropertyName("correctAnswers")]
    public int CorrectAnswers { get; set; }

    [JsonPropertyName("totalQuestions")]
    public int TotalQuestions { get; set; } = 5;
}

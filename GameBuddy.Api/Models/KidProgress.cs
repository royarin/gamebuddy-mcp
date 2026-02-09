using System.Text.Json.Serialization;

namespace GameBuddy.Api.Models;

/// <summary>
/// Represents a single quiz session for tracking progress trends.
/// A session can cover multiple countries and tracks the final difficulty level reached.
/// </summary>
public class QuizSession
{
    [JsonPropertyName("sessionId")]
    public string SessionId { get; set; } = string.Empty;

    [JsonPropertyName("countriesCovered")]
    public List<string> CountriesCovered { get; set; } = new();

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("totalQuestions")]
    public int TotalQuestions { get; set; }

    [JsonPropertyName("correctAnswers")]
    public int CorrectAnswers { get; set; }

    [JsonPropertyName("successRate")]
    public double SuccessRate { get; set; }
}

/// <summary>
/// Represents a country's progress record for a kid with aggregated totals.
/// </summary>
public class CountryProgress
{
    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("totalCorrectAnswers")]
    public int TotalCorrectAnswers { get; set; }

    [JsonPropertyName("sessionsCompleted")]
    public int SessionsCompleted { get; set; }

    [JsonPropertyName("averageSuccessRate")]
    public double AverageSuccessRate { get; set; }
}

/// <summary>
/// Represents the overall progress for a kid, tracking quiz sessions and country-level statistics.
/// </summary>
public class KidProgress
{
    [JsonPropertyName("nickname")]
    public string Nickname { get; set; } = string.Empty;

    [JsonPropertyName("countries")]
    public List<CountryProgress> Countries { get; set; } = new();

    [JsonPropertyName("quizSessions")]
    public List<QuizSession> QuizSessions { get; set; } = new();

    [JsonPropertyName("totalQuizzesCompleted")]
    public int TotalQuizzesCompleted { get; set; }

    [JsonPropertyName("overallAverageSuccessRate")]
    public double OverallAverageSuccessRate { get; set; }

    [JsonPropertyName("lastUpdated")]
    public DateTime LastUpdated { get; set; }
}

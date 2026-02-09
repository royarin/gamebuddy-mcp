namespace GameBuddy.MCP.Models;

public class KidProgressResponse
{
    public string Nickname { get; set; } = string.Empty;
    public List<CountryProgressItem> Countries { get; set; } = new();
    public List<QuizSessionItem> QuizSessions { get; set; } = new();
    public int TotalQuizzesCompleted { get; set; }
    public double OverallAverageSuccessRate { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class CountryProgressItem
{
    public string Country { get; set; } = string.Empty;
    public int TotalCorrectAnswers { get; set; }
    public int SessionsCompleted { get; set; }
    public double AverageSuccessRate { get; set; }
}

public class QuizSessionItem
{
    public string SessionId { get; set; } = string.Empty;
    public List<string> CountriesCovered { get; set; } = new();
    public DateTime Date { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public double SuccessRate { get; set; }
}

public class AllProgressResponse
{
    public int TotalKids { get; set; }
    public List<KidProgressResponse> Progress { get; set; } = new();
}

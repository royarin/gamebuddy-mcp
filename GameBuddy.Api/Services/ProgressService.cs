using GameBuddy.Api.Models;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace GameBuddy.Api.Services;

/// <summary>
/// Service for managing kid progress tracking using JSON file storage.
/// </summary>
public class ProgressService : IProgressService
{
    private readonly string _progressDirectory;
    private readonly ILogger<ProgressService> _logger;

    public ProgressService(ILogger<ProgressService> logger)
    {
        _logger = logger;
        _progressDirectory = Path.Combine(AppContext.BaseDirectory, "Data", "Progress");
        
        // Ensure progress directory exists
        if (!Directory.Exists(_progressDirectory))
        {
            Directory.CreateDirectory(_progressDirectory);
            _logger.LogInformation("Created progress directory: {ProgressDirectory}", _progressDirectory);
        }
    }

    /// <summary>
    /// Get the progress for a specific kid.
    /// </summary>
    public KidProgress? GetKidProgress(string nickname)
    {
        try
        {
            var progressFile = GetProgressFilePath(nickname);

            if (!File.Exists(progressFile))
            {
                _logger.LogInformation("No progress file found for kid: {Nickname}", nickname);
                return null;
            }

            var json = File.ReadAllText(progressFile);
            var progress = JsonSerializer.Deserialize<KidProgress>(json);
            
            _logger.LogInformation("Retrieved progress for kid: {Nickname}", nickname);
            return progress;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving progress for kid: {Nickname}", nickname);
            return null;
        }
    }

    /// <summary>
    /// Record quiz completion and update kid's progress.
    /// </summary>
    public KidProgress RecordQuizCompletion(string nickname, List<string> countriesCovered, int correctAnswers, int totalQuestions = 5)
    {
        try
        {
            var progress = GetKidProgress(nickname) ?? new KidProgress
            {
                Nickname = nickname,
                Countries = new(),
                QuizSessions = new(),
                TotalQuizzesCompleted = 0,
                OverallAverageSuccessRate = 0
            };

            // Calculate success rate for this quiz
            double successRate = totalQuestions > 0 ? (correctAnswers * 100.0) / totalQuestions : 0;

            // Create a new quiz session (not tied to a single country)
            var quizSession = new QuizSession
            {
                SessionId = Guid.NewGuid().ToString(),
                CountriesCovered = countriesCovered,
                Date = DateTime.UtcNow,
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctAnswers,
                SuccessRate = successRate,

            };

            // Add session to the list
            progress.QuizSessions.Add(quizSession);

            // Update country progress for each country covered in this session
            foreach (var country in countriesCovered.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                var countryProgress = progress.Countries.FirstOrDefault(c => c.Country.Equals(country, StringComparison.OrdinalIgnoreCase));
                
                if (countryProgress == null)
                {
                    countryProgress = new CountryProgress 
                    { 
                        Country = country, 
                        TotalCorrectAnswers = 0,
                        SessionsCompleted = 0,
                        AverageSuccessRate = 0
                    };
                    progress.Countries.Add(countryProgress);
                }

                // Track that a session covered this country
                countryProgress.SessionsCompleted++;
                // Distribute correct answers proportionally across countries covered
                countryProgress.TotalCorrectAnswers += correctAnswers / countriesCovered.Count;
            }

            // Recalculate country average success rates
            foreach (var country in progress.Countries)
            {
                var countrySessions = progress.QuizSessions
                    .Where(s => s.CountriesCovered.Any(c => c.Equals(country.Country, StringComparison.OrdinalIgnoreCase)))
                    .Select(s => s.SuccessRate)
                    .ToList();
                country.AverageSuccessRate = countrySessions.Count > 0 
                    ? countrySessions.Average() 
                    : 0;
            }
            
            // Update overall statistics
            progress.TotalQuizzesCompleted++;
            
            // Recalculate overall average success rate
            var allSuccessRates = progress.QuizSessions
                .Select(s => s.SuccessRate)
                .ToList();
            progress.OverallAverageSuccessRate = allSuccessRates.Count > 0 
                ? allSuccessRates.Average() 
                : 0;
            
            // Update last modified timestamp
            progress.LastUpdated = DateTime.UtcNow;

            // Save to file
            SaveProgress(progress);

            var countriesStr = string.Join(", ", countriesCovered);
            _logger.LogInformation("Recorded quiz completion for kid: {Nickname}, Countries: {Countries}, CorrectAnswers: {CorrectAnswers}/{TotalQuestions}, SuccessRate: {SuccessRate:F2}%", 
                nickname, countriesStr, correctAnswers, totalQuestions, successRate);

            return progress;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording quiz completion for kid: {Nickname}", nickname);
            throw;
        }
    }

    /// <summary>
    /// Get progress for all kids.
    /// </summary>
    public List<KidProgress> GetAllProgress()
    {
        try
        {
            var allProgress = new List<KidProgress>();

            if (!Directory.Exists(_progressDirectory))
            {
                return allProgress;
            }

            var files = Directory.GetFiles(_progressDirectory, "*.json");

            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                var progress = JsonSerializer.Deserialize<KidProgress>(json);
                
                if (progress != null)
                {
                    allProgress.Add(progress);
                }
            }

            _logger.LogInformation("Retrieved progress for {Count} kids", allProgress.Count);
            return allProgress;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all progress data");
            return new List<KidProgress>();
        }
    }

    /// <summary>
    /// Reset progress for a specific kid.
    /// </summary>
    public void ResetKidProgress(string nickname)
    {
        try
        {
            var progressFile = GetProgressFilePath(nickname);

            if (File.Exists(progressFile))
            {
                File.Delete(progressFile);
                _logger.LogInformation("Reset progress for kid: {Nickname}", nickname);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting progress for kid: {Nickname}", nickname);
            throw;
        }
    }

    private void SaveProgress(KidProgress progress)
    {
        var progressFile = GetProgressFilePath(progress.Nickname);
        var json = JsonSerializer.Serialize(progress, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(progressFile, json);
    }

    private string GetProgressFilePath(string nickname)
    {
        return Path.Combine(_progressDirectory, $"{nickname}_progress.json");
    }
}

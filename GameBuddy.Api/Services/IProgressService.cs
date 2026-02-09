using GameBuddy.Api.Models;

namespace GameBuddy.Api.Services;

/// <summary>
/// Service interface for managing kid progress tracking.
/// </summary>
public interface IProgressService
{
    /// <summary>
    /// Get the progress for a specific kid.
    /// </summary>
    KidProgress? GetKidProgress(string nickname);

    /// <summary>
    /// Record quiz completion and update kid's progress with session tracking.
    /// Supports multiple countries covered in a single quiz session.
    /// </summary>
    KidProgress RecordQuizCompletion(string nickname, List<string> countriesCovered, int correctAnswers, int totalQuestions = 5);

    /// <summary>
    /// Get progress for all kids.
    /// </summary>
    List<KidProgress> GetAllProgress();

    /// <summary>
    /// Reset progress for a specific kid.
    /// </summary>
    void ResetKidProgress(string nickname);
}

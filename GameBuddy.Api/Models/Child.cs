namespace GameBuddy.Api.Models;

/// <summary>
/// Represents a child (learner) in the system.
/// </summary>
public class Child
{
    public string Nickname { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string PreferredDifficulty { get; set; } = "easy";
    public int PreferredQuizLength { get; set; } = 5;
    public List<string> TopicsOfInterest { get; set; } = new();
}

using GameBuddy.Api.Models;

namespace GameBuddy.Api.Data;

/// <summary>
/// In-memory data store for GameBuddy data.
/// </summary>
public class GameBuddyData
{
    public List<Child> Children { get; set; } = new();
}

using GameBuddy.Api.Data;

namespace GameBuddy.Api.Services;

public class GameBuddyAgentService
{
    private readonly IDataService _dataService;

    public GameBuddyAgentService(IDataService dataService)
    {
        _dataService = dataService;
    }

    public List<string> GetAllKids()
    {
        var data = _dataService.GetData();
        return data.Children.Select(c => c.Nickname).ToList();
    }

    public object? GetKidProfile(string nickname)
    {
        var data = _dataService.GetData();
        var child = data.Children.FirstOrDefault(c => 
            c.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase));
        
        if (child == null)
        {
            return null;
        }
        
        return new 
        { 
            name = child.Name,
            age = child.Age
        };
    }

    public object? GetKidPreferences(string nickname)
    {
        var data = _dataService.GetData();
        var child = data.Children.FirstOrDefault(c => 
            c.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase));
        
        if (child == null)
        {
            return null;
        }
        
        return new 
        { 
            preferredDifficulty = child.PreferredDifficulty,
            preferredQuizLength = child.PreferredQuizLength,
            topicsOfInterest = child.TopicsOfInterest
        };
    }
}

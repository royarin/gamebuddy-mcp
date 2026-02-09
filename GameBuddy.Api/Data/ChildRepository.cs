using GameBuddy.Api.Models;
using System.Text.Json;

namespace GameBuddy.Api.Data;

public class ChildRepository : IChildRepository
{
    private readonly GameBuddyData _data;
    private readonly ILogger<ChildRepository> _logger;

    public ChildRepository(ILogger<ChildRepository> logger)
    {
        _logger = logger;
        _data = LoadData();
    }

    public List<Child> GetAllChildren() => _data.Children;

    public Child? GetChildByNickname(string nickname)
    {
        return _data.Children.FirstOrDefault(c => 
            c.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase));
    }

    private GameBuddyData LoadData()
    {
        try
        {
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "gamebuddy-data.json");
            
            if (!File.Exists(jsonPath))
            {
                _logger.LogError("JSON data file not found at {JsonPath}", jsonPath);
                throw new FileNotFoundException($"Data file not found at {jsonPath}");
            }

            _logger.LogInformation("Loading data from JSON file: {JsonPath}", jsonPath);
            var jsonContent = File.ReadAllText(jsonPath);
            var data = JsonSerializer.Deserialize<GameBuddyData>(jsonContent, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });
            
            if (data == null)
            {
                _logger.LogError("Failed to deserialize data from JSON file");
                throw new InvalidOperationException("Failed to load data from JSON file");
            }
            
            _logger.LogInformation("Successfully loaded {ChildCount} children", data.Children.Count);
            
            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Critical error loading data from JSON");
            throw;
        }
    }
}

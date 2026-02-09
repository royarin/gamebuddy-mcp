using GameBuddy.Api.Models;
using System.Text.Json;

namespace GameBuddy.Api.Data;

/// <summary>
/// In-memory data store for GameBuddy data.
/// </summary>
public class InMemoryDataStore
{
    public List<Child> Children { get; set; } = new();
}

public interface IDataService
{
    InMemoryDataStore GetData();
}

public class DataService : IDataService
{
    private readonly InMemoryDataStore _data;
    private readonly ILogger<DataService> _logger;

    public DataService(ILogger<DataService> logger)
    {
        _logger = logger;
        _data = LoadData();
    }

    public InMemoryDataStore GetData() => _data;

    private InMemoryDataStore LoadData()
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
            var data = JsonSerializer.Deserialize<InMemoryDataStore>(jsonContent, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });
            
            if (data == null)
            {
                _logger.LogError("Failed to deserialize data from JSON file");
                throw new InvalidOperationException("Failed to load data from JSON file");
            }
            
            _logger.LogInformation("Successfully loaded {ChildCount} children",
                data.Children.Count);
            
            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Critical error loading data from JSON");
            throw;
        }
    }
}

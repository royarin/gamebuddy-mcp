using System;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using GameBuddy.MCP.Models;
using ModelContextProtocol.Server;

namespace GameBuddy.MCP.Tools;

[McpServerToolType]
public sealed class GameBuddyTool
{
    private readonly HttpClient _httpClient;

    public GameBuddyTool(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("GameBuddyApi");
    }

    [McpServerTool(Name = "list_all_children_by_nicknames")]
    [Description("Lists all available children in the GameBuddy system by their nicknames (e.g., 'alex', 'mila', 'sam'). These nicknames ARE the identifiers - use them directly in other tools without any ID conversion.")]
    public async Task<object> GetAllKids()
    {
        try
        {
            var response = await _httpClient.GetAsync("/kids");

            if (!response.IsSuccessStatusCode)
            {
                return new ErrorResponse("Failed to retrieve kids list");
            }

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            var kids = result.GetProperty("kids").EnumerateArray()
                .Select(k => k.GetString())
                .Where(k => k != null)
                .ToList();

            return new KidsListResponse { Kids = kids! };
        }
        catch (Exception ex)
        {
            return new ErrorResponse(ex.Message);
        }
    }

    [McpServerTool(Name = "get_child_profile_by_nickname")]
    [Description("Gets basic profile (name and age) for a child by their nickname. Pass the nickname directly (e.g., 'mila', 'alex', 'sam'). Does NOT include preferences or progress - use get_child_preferences_by_nickname or get_child_progress_by_nickname for those.")]
    public async Task<object> GetKidProfile([Description("Child's nickname (e.g., 'mila', 'alex', 'sam') - just pass the nickname string directly")] string nickname)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/kids/{nickname}/profile");

            if (!response.IsSuccessStatusCode)
            {
                return new ErrorResponse($"Kid with nickname '{nickname}' not found");
            }

            var profile = await response.Content.ReadFromJsonAsync<JsonElement>();
            var name = profile.GetProperty("name").GetString();
            var age = profile.GetProperty("age").GetInt32();

            return new KidProfileResponse
            {
                Nickname = nickname,
                Name = name!,
                Age = age
            };
        }
        catch (Exception ex)
        {
            return new ErrorResponse(ex.Message);
        }
    }

    [McpServerTool(Name = "get_child_preferences_by_nickname")]
    [Description("Gets learning preferences (difficulty level, quiz length, topics) for a child by their nickname. Pass the nickname directly (e.g., 'mila', 'alex', 'sam'). Does NOT include progress data - use get_child_progress_by_nickname for that.")]
    public async Task<object> GetKidPreferences([Description("Child's nickname (e.g., 'mila', 'alex', 'sam') - just pass the nickname string directly")] string nickname)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/kids/{nickname}/preferences");

            if (!response.IsSuccessStatusCode)
            {
                return new ErrorResponse($"Kid with nickname '{nickname}' not found");
            }

            var preferences = await response.Content.ReadFromJsonAsync<JsonElement>();
            var difficulty = preferences.GetProperty("preferredDifficulty").GetString();
            var quizLength = preferences.GetProperty("preferredQuizLength").GetInt32();
            var topics = preferences.GetProperty("topicsOfInterest").EnumerateArray()
                .Select(t => t.GetString())
                .Where(t => t != null)
                .ToList();

            return new KidPreferencesResponse
            {
                Nickname = nickname,
                PreferredDifficulty = difficulty!,
                PreferredQuizLength = quizLength,
                TopicsOfInterest = topics!
            };
        }
        catch (Exception ex)
        {
            return new ErrorResponse(ex.Message);
        }
    }

    [McpServerTool(Name = "get_child_progress_by_nickname")]
    [Description("Gets progress data for ONE specific child by their nickname. Pass the nickname directly (e.g., 'mila', 'alex', 'sam'). Returns country statistics, quiz session history, and performance metrics for that one child. For multiple children or comparisons, use get_all_children_progress instead.")]
    public async Task<object> GetKidProgress([Description("Child's nickname (e.g., 'mila', 'alex', 'sam') - just pass the nickname string directly")] string nickname)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/kids/{nickname}/progress");

            if (!response.IsSuccessStatusCode)
            {
                return new ErrorResponse($"Failed to retrieve progress for kid '{nickname}'");
            }

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            var countries = new List<CountryProgressItem>();
            var quizSessions = new List<QuizSessionItem>();

            if (result.TryGetProperty("countries", out var countriesElement))
            {
                foreach (var countryElement in countriesElement.EnumerateArray())
                {
                    countries.Add(new CountryProgressItem
                    {
                        Country = countryElement.GetProperty("country").GetString() ?? string.Empty,
                        TotalCorrectAnswers = countryElement.GetProperty("totalCorrectAnswers").GetInt32(),
                        SessionsCompleted = countryElement.GetProperty("sessionsCompleted").GetInt32(),
                        AverageSuccessRate = countryElement.GetProperty("averageSuccessRate").GetDouble()
                    });
                }
            }

            if (result.TryGetProperty("quizSessions", out var sessionsElement))
            {
                foreach (var sessionElement in sessionsElement.EnumerateArray())
                {
                    var countriesCovered = new List<string>();
                    if (sessionElement.TryGetProperty("countriesCovered", out var coveredElement))
                    {
                        foreach (var countryElement in coveredElement.EnumerateArray())
                        {
                            var countryName = countryElement.GetString();
                            if (!string.IsNullOrEmpty(countryName))
                            {
                                countriesCovered.Add(countryName);
                            }
                        }
                    }

                    quizSessions.Add(new QuizSessionItem
                    {
                        SessionId = sessionElement.GetProperty("sessionId").GetString() ?? string.Empty,
                        CountriesCovered = countriesCovered,
                        Date = sessionElement.GetProperty("date").GetDateTime(),
                        TotalQuestions = sessionElement.GetProperty("totalQuestions").GetInt32(),
                        CorrectAnswers = sessionElement.GetProperty("correctAnswers").GetInt32(),
                        SuccessRate = sessionElement.GetProperty("successRate").GetDouble()
                    });
                }
            }

            return new KidProgressResponse
            {
                Nickname = result.GetProperty("nickname").GetString() ?? nickname,
                Countries = countries,
                QuizSessions = quizSessions,
                TotalQuizzesCompleted = result.GetProperty("totalQuizzesCompleted").GetInt32(),
                OverallAverageSuccessRate = result.GetProperty("overallAverageSuccessRate").GetDouble(),
                LastUpdated = result.GetProperty("lastUpdated").GetDateTime()
            };
        }
        catch (Exception ex)
        {
            return new ErrorResponse(ex.Message);
        }
    }

    [McpServerTool(Name = "get_all_children_progress")]
    [Description("Gets progress data for ALL children in the system at once. Returns country statistics and quiz session history for every child. Use ONLY when comparing multiple children, asking about 'all', or requesting aggregate data. For a single child, use get_child_progress_by_nickname instead.")]
    public async Task<object> GetAllKidsProgress()
    {
        try
        {
            var response = await _httpClient.GetAsync("/progress/all");

            if (!response.IsSuccessStatusCode)
            {
                return new ErrorResponse("Failed to retrieve all progress data");
            }

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            var allProgress = new List<KidProgressResponse>();

            if (result.TryGetProperty("progress", out var progressElement))
            {
                foreach (var kidProgress in progressElement.EnumerateArray())
                {
                    var countries = new List<CountryProgressItem>();
                    var quizSessions = new List<QuizSessionItem>();
                    
                    if (kidProgress.TryGetProperty("countries", out var countriesElement))
                    {
                        foreach (var countryElement in countriesElement.EnumerateArray())
                        {
                            countries.Add(new CountryProgressItem
                            {
                                Country = countryElement.GetProperty("country").GetString() ?? string.Empty,
                                TotalCorrectAnswers = countryElement.GetProperty("totalCorrectAnswers").GetInt32(),
                                SessionsCompleted = countryElement.GetProperty("sessionsCompleted").GetInt32(),
                                AverageSuccessRate = countryElement.GetProperty("averageSuccessRate").GetDouble()
                            });
                        }
                    }

                    if (kidProgress.TryGetProperty("quizSessions", out var sessionsElement))
                    {
                        foreach (var sessionElement in sessionsElement.EnumerateArray())
                        {
                            var countriesCovered = new List<string>();
                            if (sessionElement.TryGetProperty("countriesCovered", out var coveredElement))
                            {
                                foreach (var countryElement in coveredElement.EnumerateArray())
                                {
                                    var countryName = countryElement.GetString();
                                    if (!string.IsNullOrEmpty(countryName))
                                    {
                                        countriesCovered.Add(countryName);
                                    }
                                }
                            }

                            quizSessions.Add(new QuizSessionItem
                            {
                                SessionId = sessionElement.GetProperty("sessionId").GetString() ?? string.Empty,
                                CountriesCovered = countriesCovered,
                                Date = sessionElement.GetProperty("date").GetDateTime(),
                                TotalQuestions = sessionElement.GetProperty("totalQuestions").GetInt32(),
                                CorrectAnswers = sessionElement.GetProperty("correctAnswers").GetInt32(),
                                SuccessRate = sessionElement.GetProperty("successRate").GetDouble()
                            });
                        }
                    }

                    allProgress.Add(new KidProgressResponse
                    {
                        Nickname = kidProgress.GetProperty("nickname").GetString() ?? string.Empty,
                        Countries = countries,
                        QuizSessions = quizSessions,
                        TotalQuizzesCompleted = kidProgress.GetProperty("totalQuizzesCompleted").GetInt32(),
                        OverallAverageSuccessRate = kidProgress.GetProperty("overallAverageSuccessRate").GetDouble(),
                        LastUpdated = kidProgress.GetProperty("lastUpdated").GetDateTime()
                    });
                }
            }

            return new AllProgressResponse
            {
                TotalKids = allProgress.Count,
                Progress = allProgress
            };
        }
        catch (Exception ex)
        {
            return new ErrorResponse(ex.Message);
        }
    }

    [McpServerTool(Name = "record_child_quiz_completion_by_nickname")]
    [Description("Records a quiz session completion for a child by their nickname. Pass the nickname directly (e.g., 'mila', 'alex', 'sam'). Updates progress tracking with session data and country statistics.")]
    public async Task<object> RecordQuizCompletion(
        [Description("Child's nickname (e.g., 'mila', 'alex', 'sam') - just pass the nickname string directly")] string nickname, 
        [Description("Comma-separated country names covered in this quiz (e.g., 'France' or 'France,Germany,Italy')")] string countries, 
        [Description("Number of correct answers in this quiz session")] int correctAnswers,
        [Description("Total number of questions in this quiz session (default: 5)")] int totalQuestions = 5)
    {
        try
        {
            var countriesList = countries.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
            
            var requestBody = new
            {
                countriesCovered = countriesList,
                correctAnswers,
                totalQuestions
            };

            var response = await _httpClient.PostAsJsonAsync($"/kids/{nickname}/quiz-completion", requestBody);

            if (!response.IsSuccessStatusCode)
            {
                return new ErrorResponse($"Failed to record quiz completion for kid '{nickname}'");
            }

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            var message = result.GetProperty("message").GetString();

            if (result.TryGetProperty("progress", out var progressElement))
            {
                var countryProgressList = new List<CountryProgressItem>();
                var quizSessions = new List<QuizSessionItem>();
                
                if (progressElement.TryGetProperty("countries", out var countriesElement))
                {
                    foreach (var countryElement in countriesElement.EnumerateArray())
                    {
                        countryProgressList.Add(new CountryProgressItem
                        {
                            Country = countryElement.GetProperty("country").GetString() ?? string.Empty,
                            TotalCorrectAnswers = countryElement.GetProperty("totalCorrectAnswers").GetInt32(),
                            SessionsCompleted = countryElement.GetProperty("sessionsCompleted").GetInt32(),
                            AverageSuccessRate = countryElement.GetProperty("averageSuccessRate").GetDouble()
                        });
                    }
                }

                if (progressElement.TryGetProperty("quizSessions", out var sessionsElement))
                {
                    foreach (var sessionElement in sessionsElement.EnumerateArray())
                    {
                        var countriesCovered = new List<string>();
                        if (sessionElement.TryGetProperty("countriesCovered", out var coveredElement))
                        {
                            foreach (var countryElement in coveredElement.EnumerateArray())
                            {
                                var countryName = countryElement.GetString();
                                if (!string.IsNullOrEmpty(countryName))
                                {
                                    countriesCovered.Add(countryName);
                                }
                            }
                        }

                        quizSessions.Add(new QuizSessionItem
                        {
                            SessionId = sessionElement.GetProperty("sessionId").GetString() ?? string.Empty,
                            CountriesCovered = countriesCovered,
                            Date = sessionElement.GetProperty("date").GetDateTime(),
                            TotalQuestions = sessionElement.GetProperty("totalQuestions").GetInt32(),
                            CorrectAnswers = sessionElement.GetProperty("correctAnswers").GetInt32(),
                            SuccessRate = sessionElement.GetProperty("successRate").GetDouble()
                        });
                    }
                }

                return new KidProgressResponse
                {
                    Nickname = progressElement.GetProperty("nickname").GetString() ?? nickname,
                    Countries = countryProgressList,
                    QuizSessions = quizSessions,
                    TotalQuizzesCompleted = progressElement.GetProperty("totalQuizzesCompleted").GetInt32(),
                    OverallAverageSuccessRate = progressElement.GetProperty("overallAverageSuccessRate").GetDouble(),
                    LastUpdated = progressElement.GetProperty("lastUpdated").GetDateTime()
                };
            }

            return new { message };
        }
        catch (Exception ex)
        {
            return new ErrorResponse(ex.Message);
        }
    }
}

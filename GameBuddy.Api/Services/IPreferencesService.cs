using GameBuddy.Api.Models;

namespace GameBuddy.Api.Services;

public interface IPreferencesService
{
    KidPreferencesDto? GetKidPreferences(string nickname);
}

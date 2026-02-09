using GameBuddy.Api.Data;
using GameBuddy.Api.Models;

namespace GameBuddy.Api.Services;

public class PreferencesService : IPreferencesService
{
    private readonly IChildRepository _childRepository;

    public PreferencesService(IChildRepository childRepository)
    {
        _childRepository = childRepository;
    }

    public KidPreferencesDto? GetKidPreferences(string nickname)
    {
        var child = _childRepository.GetChildByNickname(nickname);
        
        if (child == null)
        {
            return null;
        }
        
        return new KidPreferencesDto
        { 
            PreferredDifficulty = child.PreferredDifficulty,
            PreferredQuizLength = child.PreferredQuizLength,
            TopicsOfInterest = child.TopicsOfInterest
        };
    }
}

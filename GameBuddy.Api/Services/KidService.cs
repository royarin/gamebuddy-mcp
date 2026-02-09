using GameBuddy.Api.Data;
using GameBuddy.Api.Models;

namespace GameBuddy.Api.Services;

public class KidService : IKidService
{
    private readonly IChildRepository _childRepository;

    public KidService(IChildRepository childRepository)
    {
        _childRepository = childRepository;
    }

    public List<string> GetAllKids()
    {
        var children = _childRepository.GetAllChildren();
        return children.Select(c => c.Nickname).ToList();
    }

    public KidProfileDto? GetKidProfile(string nickname)
    {
        var child = _childRepository.GetChildByNickname(nickname);
        
        if (child == null)
        {
            return null;
        }
        
        return new KidProfileDto
        { 
            Name = child.Name,
            Age = child.Age
        };
    }
}

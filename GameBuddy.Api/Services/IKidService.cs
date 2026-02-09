using GameBuddy.Api.Models;

namespace GameBuddy.Api.Services;

public interface IKidService
{
    List<string> GetAllKids();
    KidProfileDto? GetKidProfile(string nickname);
}

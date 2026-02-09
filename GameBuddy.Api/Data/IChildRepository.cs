using GameBuddy.Api.Models;

namespace GameBuddy.Api.Data;

public interface IChildRepository
{
    List<Child> GetAllChildren();
    Child? GetChildByNickname(string nickname);
}

using TimeTrackerApi.Library.Models;

namespace TimeTrackerApi.Library.Repositories.Interfaces;
public interface IUserRepo
{
    Task<User> GetUserByIdAsync(string id);
}
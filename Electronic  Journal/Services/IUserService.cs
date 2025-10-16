using Electronic__Journal.Models;

namespace Electronic__Journal.Services
{
    public interface IUserService
    {
        Task<User> LoginAsync(string login, string password);
    }
}

using Terepnaplo.Models;

namespace Terepnaplo.Contracts
{
    public interface IUserService
    {
        Task<bool> LogInUser(string email, string password);

        Task<bool> RegisterUser(UserDto user);  

        UserDto? GetLoggedInUser();

        void LogOutUser();
    }
}

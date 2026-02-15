using MadarfigyeloApp.Models;
using Refit;

namespace MadarfigyeloApp.Contracts
{
    public interface IUserService
    {
        Task<bool> LogInUser(string email, string password);

        Task<bool> RegisterUser(UserDto user);  

        UserDto? GetLoggedInUser();

        void LogOutUser();
    }
}

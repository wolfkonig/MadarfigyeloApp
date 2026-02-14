using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.Contracts
{
    public interface IUserService
    {
        Task<bool> LogInUser(string username, string password);

        Task<bool> RegisterUser(User user);  

        User? GetLoggedInUser();

        void LogOutUser();
    }
}

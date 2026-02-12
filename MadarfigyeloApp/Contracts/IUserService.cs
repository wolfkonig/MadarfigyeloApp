using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.Contracts
{
    public interface IUserService
    {
        Task<bool> LogInUser(string username, string password);

        Task<User?> GetLoggedInUser();

        Task<bool> LogOutUser();
    }
}

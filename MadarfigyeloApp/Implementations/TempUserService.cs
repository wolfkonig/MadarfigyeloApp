using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.Implementations
{
    public class TempUserService : IUserService
    {
        public async Task<User?> GetLoggedInUser()
        {
            var username = await SecureStorage.Default.GetAsync(Constants.KeyLoggedInUserName);

            if (string.IsNullOrEmpty(username))
            {
                return null;
            }
            var user = new User() 
            { 
                Name = username
            };

            return user;
        }

        public async Task<bool> LogInUser(string username, string password)
        {
            if (!string.IsNullOrEmpty(username))
            {
                await SecureStorage.SetAsync(Constants.KeyLoggedInUserName, username);
                return true;
            }
            return false;
        }

        public Task<bool> LogOutUser()
        {
            SecureStorage.Remove(Constants.KeyLoggedInUserName);
            return Task.FromResult(true);
        }
    }
}

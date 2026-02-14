using MadarfigyeloApp.API;
using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.Contracts
{
    public class UserService : IUserService
    {
        private readonly IAuthApi _authApi;

        public UserService(IAuthApi authApi)    
        {
            _authApi = authApi ?? throw new ArgumentNullException(nameof(authApi));
        }

        public User? GetLoggedInUser()
        {
            var email = Preferences.Default.Get(Constants.KeyLoggedInUserEmail, string.Empty);
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            var user = new User(email)
            {
                FirstName = Preferences.Default.Get(Constants.KeyLoggedInUserFirstName, string.Empty),
                LastName = Preferences.Default.Get(Constants.KeyLoggedInUserLastName, string.Empty)
            };
            return user;
        }

        public async Task<bool> LogInUser(string username, string password)
        {
            var authResponse = await _authApi.LogInUser(username, password);

            if (authResponse?.IsSuccess == true)
            {
                Preferences.Default.Set(Constants.KeyLoggedInUserEmail, authResponse.Email);
                await SecureStorage.Default.SetAsync(Constants.KeyLoggedInUserPassword, password);
                await SecureStorage.Default.SetAsync(Constants.KeyLoggedInUserToken, authResponse.Token);
                Preferences.Default.Set(Constants.KeyLoggedInUserTokenExpDate, authResponse.Expiration);
                return true;    
            }
            return false;
        }

        public void LogOutUser()
        {
            Preferences.Default.Remove(Constants.KeyLoggedInUserEmail);
            Preferences.Default.Remove(Constants.KeyLoggedInUserFirstName);
            Preferences.Default.Remove(Constants.KeyLoggedInUserLastName);
            SecureStorage.Default.Remove(Constants.KeyLoggedInUserPassword);
            SecureStorage.Default.Remove(Constants.KeyLoggedInUserToken);
            Preferences.Default.Remove(Constants.KeyLoggedInUserTokenExpDate);
        }

        public async Task<bool> RegisterUser(User user)
        {
            var authResponse = await _authApi.RegisterUser(user);
            return authResponse?.IsSuccess == true;
        }
    }
}

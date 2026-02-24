using MadarfigyeloApp.API;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Resources;
using Refit;
using System.Text;

namespace MadarfigyeloApp.Implementations
{
    public class UserService : IUserService
    {
        private readonly IAuthApi _authApi;
        private readonly ILoggerService _logger;  

        public UserService(IAuthApi authApi, ILoggerService logger)    
        {
            _authApi = authApi ?? throw new ArgumentNullException(nameof(authApi));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public UserDto? GetLoggedInUser()
        {
            var email = Preferences.Default.Get(Constants.KeyLoggedInUserEmail, string.Empty);
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            var user = new UserDto(email)
            {
                FirstName = Preferences.Default.Get(Constants.KeyLoggedInUserFirstName, string.Empty),
                LastName = Preferences.Default.Get(Constants.KeyLoggedInUserLastName, string.Empty)
            };
            return user;
        }

        public async Task<bool> LogInUser(string email, string password)
        {
            var login = new LoginDto 
            { 
                Email = email, 
                Password = password 
            };

            var authResponse = await _authApi.LogInUser(login);
            if (authResponse.Content is AuthResponseDto tokenResponse)
            {
                Preferences.Default.Set(Constants.KeyLoggedInUserEmail, tokenResponse.Email);
                await SecureStorage.Default.SetAsync(Constants.KeyLoggedInUserPassword, password);
                await SecureStorage.Default.SetAsync(Constants.KeyLoggedInUserToken, tokenResponse.Token);
                Preferences.Default.Set(Constants.KeyLoggedInUserTokenExpDate, tokenResponse.Expiration);
                return true;
            }

            _logger.LogError($"Failed to log in user {email}. ERROR: {authResponse.StatusCode}. Details: {authResponse?.Error?.Content}");
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

        public async Task<bool> RegisterUser(UserDto user)
        {
            var authResponse = await _authApi.RegisterUser(user);

            if (authResponse.Content is AuthResponseDto tokenResponse)
            {
                Preferences.Default.Set(Constants.KeyLoggedInUserEmail, tokenResponse.Email);
                await SecureStorage.Default.SetAsync(Constants.KeyLoggedInUserPassword, user.Password);
                await SecureStorage.Default.SetAsync(Constants.KeyLoggedInUserToken, tokenResponse.Token);
                Preferences.Default.Set(Constants.KeyLoggedInUserTokenExpDate, tokenResponse.Expiration);
                return true;
            }

            _logger.LogError($"Failed to register user {user.Email}. ERROR: {authResponse.StatusCode}. Details: {authResponse?.Error?.Content}");
            return false;
        }
    }
}

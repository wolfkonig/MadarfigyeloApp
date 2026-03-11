using MadarfigyeloApp.API;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using MonkeyCache.FileStore;

namespace MadarfigyeloApp.Implementations
{
    public class UserService : IUserService
    {
        private readonly IAuthApi _authApi;
        private readonly ILoggerService _logger;  
        private readonly ISettingsService _settingsService;

        public UserService(IAuthApi authApi, ILoggerService logger, ISettingsService settingsService)    
        {
            _authApi = authApi ?? throw new ArgumentNullException(nameof(authApi));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        public UserDto? GetLoggedInUser()
        {
            var email = _settingsService.Get(Constants.KeyLoggedInUserEmail, string.Empty);
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            var user = new UserDto(email)
            {
                FirstName = _settingsService.Get(Constants.KeyLoggedInUserFirstName, string.Empty),
                LastName = _settingsService.Get(Constants.KeyLoggedInUserLastName, string.Empty)
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
                _settingsService.Set(Constants.KeyLoggedInUserEmail, tokenResponse.Email);
                await _settingsService.SecureSet(Constants.KeyLoggedInUserPassword, password);
                await _settingsService.SecureSet(Constants.KeyLoggedInUserToken, tokenResponse.Token);
                _settingsService.Set(Constants.KeyLoggedInUserTokenExpDate, tokenResponse.Expiration);
                return true;
            }

            _logger.LogError($"Failed to log in user {email}. ERROR: {authResponse.StatusCode}. Details: {authResponse?.Error?.Content}");
            return false;
        }

        public void LogOutUser()
        {
            _settingsService.Remove(Constants.KeyLoggedInUserEmail);
            _settingsService.Remove(Constants.KeyLoggedInUserFirstName);
            _settingsService.Remove(Constants.KeyLoggedInUserLastName);
            _settingsService.Remove(Constants.KeyLoggedInUserPassword);
            _settingsService.Remove(Constants.KeyLoggedInUserToken);
            _settingsService.Remove(Constants.KeyLoggedInUserTokenExpDate);

            _settingsService.SelectedOduId = 0;
            _settingsService.SelectedOdutelepId = 0;
            Barrel.Current.EmptyAll();
        }

        public async Task<bool> RegisterUser(UserDto user)
        {
            var authResponse = await _authApi.RegisterUser(user);

            if (authResponse.Content is AuthResponseDto tokenResponse)
            {
                _settingsService.Set(Constants.KeyLoggedInUserEmail, tokenResponse.Email);
                _settingsService.Set(Constants.KeyLoggedInUserTokenExpDate, tokenResponse.Expiration);
                await Task.WhenAll(
                    _settingsService.SecureSet(Constants.KeyLoggedInUserPassword, user.Password),
                    _settingsService.SecureSet(Constants.KeyLoggedInUserToken, tokenResponse.Token)
                );
                return true;
            }

            _logger.LogError($"Failed to register user {user.Email}. ERROR: {authResponse.StatusCode}. Details: {authResponse?.Error?.Content}");
            return false;
        }
    }
}

using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using System.Net.Http.Headers;


namespace MadarfigyeloApp.API;

public class TokenAuthHandler : DelegatingHandler
{
    private readonly IAuthApi _authApi;
    private readonly ILoggerService _logger;
    private readonly ISettingsService _settingsService;

    public TokenAuthHandler(IAuthApi authApi, ILoggerService logger, ISettingsService settingsService)
    {
        _authApi = authApi ?? throw new ArgumentNullException(nameof(authApi));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // 1. Retrieve the token from SecureStorage if not expired
        var expDate = _settingsService.Get(Constants.KeyLoggedInUserTokenExpDate, DateTime.MinValue);

        var token = string.Empty;
        if (expDate > DateTime.Now)
        {
            token = await _settingsService.SecureGet(Constants.KeyLoggedInUserToken);
        }
        else
        {
            // 2. If token expired, attempt to refresh it using stored credentials
            var password = await _settingsService.SecureGet(Constants.KeyLoggedInUserPassword) ?? "";
            var email = _settingsService.Get(Constants.KeyLoggedInUserEmail, string.Empty);
            var loginDto = new LoginDto
            {
                Email = email,
                Password = password
            };

            var authResponse = await _authApi.LogInUser(loginDto);

            if (authResponse.IsSuccessful && authResponse.Content is AuthResponseDto tokenResponse)
            {
                token = tokenResponse.Token;
                // Store the new token and its expiration date
                await _settingsService.SecureSet(Constants.KeyLoggedInUserToken, token);
                _settingsService.Set(Constants.KeyLoggedInUserTokenExpDate, tokenResponse.Expiration);
            }
            else
            {
                _logger.LogError("Failed to refresh token. User may need to log in again.");
                _settingsService.RemoveAll();
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("Token expired and refresh failed. Please log in again.")
                };
            }
        }

        if (!string.IsNullOrEmpty(token))
        {
            // 3. Set the Authorization header
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
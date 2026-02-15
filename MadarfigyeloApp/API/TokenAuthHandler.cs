using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace MadarfigyeloApp.API;

public class TokenAuthHandler : DelegatingHandler
{
    private readonly IAuthApi _authApi;
    private readonly ILogger _logger;

    public TokenAuthHandler(IAuthApi authApi, ILogger logger)
    {
        _authApi = authApi ?? throw new ArgumentNullException(nameof(authApi));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // 1. Retrieve the token from SecureStorage if not expired
        var expDate = Preferences.Default.Get(Constants.KeyLoggedInUserTokenExpDate, DateTime.MinValue);
        string? token;
        if (expDate > DateTime.Now)
        {
            token = await SecureStorage.Default.GetAsync(Constants.KeyLoggedInUserToken);
        }
        else
        {
            // 2. If token expired, attempt to refresh it using stored credentials
            var password = await SecureStorage.Default.GetAsync(Constants.KeyLoggedInUserPassword) ?? "";
            var email = Preferences.Default.Get(Constants.KeyLoggedInUserEmail, string.Empty);
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
                await SecureStorage.Default.SetAsync(Constants.KeyLoggedInUserToken, token);
                Preferences.Default.Set(Constants.KeyLoggedInUserTokenExpDate, tokenResponse.Expiration);
            }
            else
            {
                throw new Exception("Failed to refresh token. User may need to log in again.");
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
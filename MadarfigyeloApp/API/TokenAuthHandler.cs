using MadarfigyeloApp.Contracts;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace MadarfigyeloApp.API;

public class TokenAuthHandler : DelegatingHandler
{
    private readonly IAuthApi _authApi;

    public TokenAuthHandler(IAuthApi authApi)
    {
        _authApi = authApi ?? throw new ArgumentNullException(nameof(authApi));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // 1. Retrieve the token from SecureStorage if not expired
        var expDate = Preferences.Default.Get(Constants.KeyLoggedInUserTokenExpDate, DateTime.MinValue);
        string? token;
        if (expDate > DateTime.UtcNow)
        {
            token = await SecureStorage.Default.GetAsync(Constants.KeyLoggedInUserToken);
        }
        else
        {
            // 2. If token expired, attempt to refresh it using stored credentials
            var password = await SecureStorage.Default.GetAsync(Constants.KeyLoggedInUserPassword) ?? "";
            var email = Preferences.Default.Get(Constants.KeyLoggedInUserEmail, string.Empty);

            var authResponse = await _authApi.LogInUser(email, password);

            if (authResponse.IsSuccess)
            {
                token = authResponse.Token;
                // Store the new token and its expiration date
                await SecureStorage.Default.SetAsync(Constants.KeyLoggedInUserToken, token);
                Preferences.Default.Set(Constants.KeyLoggedInUserTokenExpDate, authResponse.Expiration);
            }
            else
            {
                throw new Exception("Token refresh failed. Please log in again.");
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
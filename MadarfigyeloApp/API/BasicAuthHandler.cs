using MadarfigyeloApp.Contracts;
using System.Net.Http.Headers;
using System.Text;

namespace MadarfigyeloApp.API;

public class BasicAuthHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // 1. Retrieve credentials from SecureStorage
        //var username = await SecureStorage.GetAsync("api_user");
        //var password = await SecureStorage.GetAsync("api_pass");

        // temporary credentials temporarily hardcoded
        var username = "11291485";
        var password = "60-dayfreetrial";

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            // 2. Format: "username:password"
            var authString = $"{username}:{password}";
            var base64Auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(authString));

            // 3. Set the Authorization header
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64Auth);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}

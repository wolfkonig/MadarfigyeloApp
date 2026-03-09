using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MonkeyCache.FileStore;
using Microsoft.Maui.Networking;
using MadarfigyeloApp.Contracts;

namespace MadarfigyeloApp.API
{
    public class CachingHandler : TokenAuthHandler
    {
        private readonly IConnectivity _connectivity;

        public CachingHandler(IAuthApi authApi, IConnectivity connectivity, ILoggerService logger) : base(authApi, logger)
        {
            _connectivity = connectivity ?? throw new ArgumentNullException(nameof(connectivity));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // 1. Only cache GET requests
            if (request.Method != HttpMethod.Get)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            string cacheKey = request.RequestUri.ToString();
            bool forceRefresh = false;

            // 2. Check for our custom bypass header
            if (request.Headers.Contains("X-Force-Refresh"))
            {
                forceRefresh = request.Headers.GetValues("X-Force-Refresh").FirstOrDefault()?.ToLower() == "true";

                // Strip the header so it doesn't get sent over the network to your backend
                request.Headers.Remove("X-Force-Refresh");
            }

            // 3. Handle Offline State (always try to return cache if offline, even if they asked for a refresh)
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                if (Barrel.Current.Exists(cacheKey)) // Ignore expiration if offline
                {
                    var cachedContent = Barrel.Current.Get<string>(cacheKey);
                    return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                    {
                        Content = new StringContent(cachedContent, System.Text.Encoding.UTF8, "application/json")
                    };
                }
                throw new HttpRequestException("No internet connection and no cached data available.");
            }

            // 4. Return cache if we are NOT forcing a refresh AND the cache is valid
            if (!forceRefresh && Barrel.Current.Exists(cacheKey) && !Barrel.Current.IsExpired(cacheKey))
            {
                var cachedContent = Barrel.Current.Get<string>(cacheKey);
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(cachedContent, System.Text.Encoding.UTF8, "application/json")
                };
            }

            // 5. Fetch fresh data (either because cache is expired, missing, or forceRefresh is true)
            try
            {
                var response = await base.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Barrel.Current.Add(key: cacheKey, data: content, expireIn: TimeSpan.FromHours(Constants.CacheInvalidationIntervalHours));
                    response.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                }

                return response;
            }
            catch (Exception)
            {
                // Fallback to stale cache if the network request fails
                if (Barrel.Current.Exists(cacheKey))
                {
                    var cachedContent = Barrel.Current.Get<string>(cacheKey);
                    return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                    {
                        Content = new StringContent(cachedContent, System.Text.Encoding.UTF8, "application/json")
                    };
                }
                throw;
            }
        }
    }
}

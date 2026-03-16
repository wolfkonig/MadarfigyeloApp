namespace MadarfigyeloApp.Implementations
{
    using MadarfigyeloApp.Contracts;
    using System;

    public sealed class LocationService : ILocationService
    {       
        public async Task<Location?> GetCurrentLocationAsync(
            bool highAccuracy = true,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default)
        {
            if (!await IsLocationEnabledAsync().ConfigureAwait(false))
                return null;

            // Ensure permission
            var status = await EnsureLocationPermissionAsync().ConfigureAwait(false);
            if (status != PermissionStatus.Granted)
                return null;

            // Build the request
            var request = new GeolocationRequest(
                highAccuracy ? GeolocationAccuracy.Best : GeolocationAccuracy.Default,
                timeout ?? TimeSpan.FromSeconds(Constants.LocationTimeoutSeconds));

            try
            {
                // Try to get a fresh fix
                var location = await Geolocation.Default.GetLocationAsync(request, cancellationToken)
                                                        .ConfigureAwait(false);
                if (location is not null)
                    return location;

                // Fallback to last known (fast, may be older)
                return await Geolocation.Default.GetLastKnownLocationAsync().ConfigureAwait(false);
            }
            catch (FeatureNotEnabledException)
            {
                // Location services off at OS level
                return null;
            }
            catch (PermissionException)
            {
                return null;
            }
            catch (OperationCanceledException)
            {
                // Timed out or caller canceled
                return null;
            }
            catch
            {
                // Swallow/Log as appropriate in your app's logging
                return null;
            }
        }

        private static async Task<bool> IsLocationEnabledAsync()
        {
            // Quick way to detect if the OS has location services enabled
            try
            {
                // Attempt a LastKnown fetch which is safe and fast; if it throws FeatureNotEnabled,
                // we know location is disabled. If it returns, services are likely enabled.
                _ = await Geolocation.Default.GetLastKnownLocationAsync().ConfigureAwait(false);
                return true;
            }
            catch (FeatureNotEnabledException)
            {
                return false;
            }
        }

        private static async Task<PermissionStatus> EnsureLocationPermissionAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>()
                                          .ConfigureAwait(false);

            if (status == PermissionStatus.Granted)
                return status;

            if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
            {
                // Optionally show your own UI explaining why you need location
                // before triggering the system prompt again.
            }

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>()
                                      .ConfigureAwait(false);
            return status;
        }
    }
}

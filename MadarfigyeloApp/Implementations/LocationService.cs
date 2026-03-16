namespace MadarfigyeloApp.Implementations
{
    using MadarfigyeloApp.Contracts;
    using System;
    using System.Collections.Generic;
    using ProjNet.CoordinateSystems;
    using ProjNet.CoordinateSystems.Transformations;

    public sealed class LocationService : ILocationService
    {
        public Location FindCentre(List<Location> locations)
        {
            // 1. Define projections
            var csFactory = new CoordinateSystemFactory();
            var ctFactory = new CoordinateTransformationFactory();

            // WGS84 (lat/lon)
            var wgs84 = csFactory.CreateGeographicCoordinateSystem(
                "WGS84",
                AngularUnit.Degrees,
                HorizontalDatum.WGS84,
                PrimeMeridian.Greenwich,
                new AxisInfo("Lon", AxisOrientationEnum.East),
                new AxisInfo("Lat", AxisOrientationEnum.North)
            );

            // UTM Zone 33N (covers Hungary well)
            var utm33 = csFactory.CreateProjectedCoordinateSystem(
                "UTM33N",
                wgs84,
                csFactory.CreateProjection("UTM33N", "Transverse_Mercator",
                    new List<ProjectionParameter>
                    {
                    new ProjectionParameter("latitude_of_origin", 0),
                    new ProjectionParameter("central_meridian", 15),
                    new ProjectionParameter("scale_factor", 0.9996),
                    new ProjectionParameter("false_easting", 500000),
                    new ProjectionParameter("false_northing", 0)
                    }),
                LinearUnit.Metre,
                new AxisInfo("East", AxisOrientationEnum.East),
                new AxisInfo("North", AxisOrientationEnum.North)
            );

            var toUtm = ctFactory.CreateFromCoordinateSystems(wgs84, utm33);
            var toWgs = ctFactory.CreateFromCoordinateSystems(utm33, wgs84);

            // 2. Convert to projected coordinates
            var xy = new List<(double x, double y)>();
            foreach (var loc in locations)
            {
                double[] p = toUtm.MathTransform.Transform([loc.Longitude, loc.Latitude]);
                xy.Add((p[0], p[1]));
            }

            // 3. Compute 2D polygon centroid (shoelace formula)
            double A = 0;
            double Cx = 0;
            double Cy = 0;

            for (int i = 0; i < xy.Count; i++)
            {
                var (x0, y0) = xy[i];
                var (x1, y1) = xy[(i + 1) % xy.Count];

                double cross = x0 * y1 - x1 * y0;
                A += cross;
                Cx += (x0 + x1) * cross;
                Cy += (y0 + y1) * cross;
            }

            A *= 0.5;
            Cx /= (6 * A);
            Cy /= (6 * A);

            // 4. Convert centroid back to lat/lon
            double[] ll = toWgs.MathTransform.Transform([Cx, Cy]);
            double lonCentroid = ll[0];
            double latCentroid = ll[1];

            return new Location(latCentroid, lonCentroid);
        }

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

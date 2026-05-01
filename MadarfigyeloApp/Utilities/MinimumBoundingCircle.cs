using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace Terepnaplo.Utilities
{
    public static class MinimumBoundingCircle
    {
        private const double MetersPerDegree = 111320.0; // Approximate meters per degree at equator

        /// <summary>
        /// Finds the minimum bounding circle for a set of geographic coordinates using Welzl's algorithm.
        /// Uses Euclidean geometry (assumes flat surface - suitable for small geographic areas).
        /// </summary>
        public static Circle? FindMinimumBoundingCircle(List<Location> locations)
        {
            if (locations == null || locations.Count == 0)
                return null;

            if (locations.Count == 1)
                return new Circle
                {
                    Center = locations[0],
                    Radius = Distance.FromMeters(0)
                };

            // Shuffle for better average performance
            var points = locations.ToList();
            Shuffle(points);

            return WelzlAlgorithm(points, new List<Location>(), points.Count);
        }

        private static Circle? WelzlAlgorithm(List<Location> points, List<Location> boundary, int n)
        {
            // Base cases
            if (n == 0 || boundary.Count == 3)
            {
                return MinimalCircleWithBoundary(boundary);
            }

            // Pick a point
            var p = points[n - 1];

            // Get the minimal circle without this point
            var circle = WelzlAlgorithm(points, boundary, n - 1);

            // If point is inside the circle, return it
            if (circle != null && IsInside(circle, p))
            {
                return circle;
            }

            // Point is outside, must be on the boundary
            boundary.Add(p);
            var result = WelzlAlgorithm(points, boundary, n - 1);
            boundary.RemoveAt(boundary.Count - 1);

            return result;
        }

        private static Circle? MinimalCircleWithBoundary(List<Location> boundary)
        {
            return boundary.Count switch
            {
                0 => null,
                1 => new Circle
                {
                    Center = boundary[0],
                    Radius = Distance.FromMeters(0)
                },
                2 => CircleFromTwoPoints(boundary[0], boundary[1]),
                3 => CircleFromThreePoints(boundary[0], boundary[1], boundary[2]),
                _ => null
            };
        }

        private static Circle CircleFromTwoPoints(Location p1, Location p2)
        {
            // Center is midpoint
            var centerLat = (p1.Latitude + p2.Latitude) / 2.0;
            var centerLon = (p1.Longitude + p2.Longitude) / 2.0;
            var center = new Location(centerLat, centerLon);

            // Radius is half the distance
            var radiusDegrees = DistanceInDegrees(p1, p2) / 2.0;
            var radiusMeters = radiusDegrees * MetersPerDegree;

            return new Circle
            {
                Center = center,
                Radius = Distance.FromMeters(radiusMeters)
            };
        }

        private static Circle? CircleFromThreePoints(Location p1, Location p2, Location p3)
        {
            // Treat lat/lon as y/x coordinates
            var x1 = p1.Longitude;
            var y1 = p1.Latitude;
            var x2 = p2.Longitude;
            var y2 = p2.Latitude;
            var x3 = p3.Longitude;
            var y3 = p3.Latitude;

            // Calculate circumcenter using standard formula
            var d = 2.0 * (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));

            if (Math.Abs(d) < 1e-10)
            {
                // Points are collinear, use two-point circle
                return CircleFromTwoPoints(p1, p2);
            }

            var ux = ((x1 * x1 + y1 * y1) * (y2 - y3) +
                      (x2 * x2 + y2 * y2) * (y3 - y1) +
                      (x3 * x3 + y3 * y3) * (y1 - y2)) / d;

            var uy = ((x1 * x1 + y1 * y1) * (x3 - x2) +
                      (x2 * x2 + y2 * y2) * (x1 - x3) +
                      (x3 * x3 + y3 * y3) * (x2 - x1)) / d;

            var center = new Location(uy, ux);

            // Calculate radius as distance to any of the three points
            var radiusDegrees = DistanceInDegrees(center, p1);
            var radiusMeters = radiusDegrees * MetersPerDegree;

            return new Circle
            {
                Center = center,
                Radius = Distance.FromMeters(radiusMeters)
            };
        }

        private static bool IsInside(Circle circle, Location point)
        {
            var distanceMeters = DistanceInDegrees(point, circle.Center) * MetersPerDegree;
            return distanceMeters <= circle.Radius.Meters * 1.000001; // Small tolerance
        }

        private static double DistanceInDegrees(Location p1, Location p2)
        {
            // Simple Euclidean distance in degrees
            var dx = p2.Longitude - p1.Longitude;
            var dy = p2.Latitude - p1.Latitude;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private static void Shuffle<T>(List<T> list)
        {
            var rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
}
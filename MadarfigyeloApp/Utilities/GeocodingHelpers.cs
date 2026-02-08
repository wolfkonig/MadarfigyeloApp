namespace MadarfigyeloApp.Utilities
{
    public static class GeocodingHelpers
    {
        public static async Task<string?> GetAddressAsync(Location location)
        {
            var placemarks = await Geocoding.Default.GetPlacemarksAsync(
                location.Latitude, location.Longitude);

            var place = placemarks?.FirstOrDefault();
            if (place is null) return null;

            return $"{place.Thoroughfare} {place.SubThoroughfare}, {place.Locality}, {place.AdminArea} {place.PostalCode}, {place.CountryName}";
        }
    }
}

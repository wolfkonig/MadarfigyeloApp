using MadarfigyeloApp.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace MadarfigyeloApp.Views;

public partial class OduMapView : BasePage
{
	public OduMapView(OduMapViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
        Loaded += OduMapView_Loaded;
	}

    private async void OduMapView_Loaded(object? sender, EventArgs e)
    {
        // Example: list of coordinates to plot
        var points = new List<(double Lat, double Lng, string Title)>
        {
            (47.4979, 19.0402, "Budapest"),  // sample
            (47.9025, 20.3772, "Eger")       // sample
        };

        // Get user location (optional) – prompts for permission the first time
        Location? myLoc = null;
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            myLoc = await Geolocation.Default.GetLocationAsync(request);
        }
        catch { /* Handle permissions/denied/timeouts gracefully */ }

        // Center map: user location if available, otherwise first pin
        var center = myLoc is not null
            ? new Location(myLoc.Latitude, myLoc.Longitude)
            : new Location(points[0].Lat, points[0].Lng);

        MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(center, Distance.FromKilometers(10)));

        // Add pins
        foreach (var p in points)
        {
            var pin = new Pin
            {
                Location = new Location(p.Lat, p.Lng),
                Label = p.Title,
                Type = PinType.Place
            };
            MyMap.Pins.Add(pin);
        }
    }

}
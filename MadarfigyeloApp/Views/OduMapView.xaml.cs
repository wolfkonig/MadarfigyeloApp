using MadarfigyeloApp.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;


namespace MadarfigyeloApp.Views;

public partial class OduMapView : BasePage
{
    public OduMapViewModel ViewModel => (OduMapViewModel)BindingContext;

    public OduMapView(OduMapViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();

        OduMap.MapClicked += (s, e) =>
        {
            ViewModel.SelectedOduId = 0;
            ViewModel.OduSelected = false;
        };


        ViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(ViewModel.CurrentLocation) && ViewModel.CurrentLocation != null && ViewModel.SelectedOdutelep.Id == 0)
            {
                OduMap.Pins.Clear();
                OduMap.MoveToRegion(MapSpan.FromCenterAndRadius(ViewModel.CurrentLocation, Distance.FromKilometers(1)));
            }

            // If odutelep is selected, add pins for all odus and move the map to show all pins
            if (e.PropertyName == nameof(ViewModel.OduList))
            {
                // Clear up old pins and selection
                OduMap.Pins.Clear();
                if (ViewModel.OduList.Count == 0 || ViewModel.SelectedOdutelep.Id == 0)
                {
                    // If no odutelep is selected, move the map to the current location
                    if (ViewModel.CurrentLocation != null)
                    {
                        OduMap.MoveToRegion(MapSpan.FromCenterAndRadius(ViewModel.CurrentLocation, Distance.FromKilometers(1)));
                    }
                    return;
                }

                var radiusKm = 0.0;
                foreach (var odu in ViewModel.OduList)
                {
                    if (odu.GpsLatitude != 0 && odu.GpsLongitude != 0)
                    {
                        var distance = Location.CalculateDistance(ViewModel.CentreMapLocation, odu.Location, DistanceUnits.Kilometers);

                        if (distance > radiusKm)
                        {
                            radiusKm = distance;
                        }

                        AddPin((double)odu.GpsLatitude, (double)odu.GpsLongitude, odu.OduAzonosito ?? "", odu.Id);
                    }
                }

                OduMap.MoveToRegion(MapSpan.FromCenterAndRadius(ViewModel.CentreMapLocation, Distance.FromKilometers(radiusKm)));
            }
        };
    }

    private void AddPin(double latitude, double longitude, string label, int oduId)
    {
        var pin = new Pin
        {
            Location = new Location(latitude, longitude),
            Label = label,
            Type = PinType.Generic           
        };

        pin.MarkerClicked += (s, e) =>
        {
            ViewModel.SelectedOduId = oduId;
            ViewModel.OduSelected = true;
        };

        OduMap.Pins.Add(pin);
    }

}
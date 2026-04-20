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
            // Clear pins if no Odutelep selected
            if (e.PropertyName is nameof(ViewModel.CurrentLocation) or nameof(ViewModel.SelectedOdutelep) 
                && ViewModel.ShowMapRegion is not null 
                && ViewModel.SelectedOdutelep.Id == 0)
            {
                OduMap.Pins.Clear();
                OduMap.MoveToRegion(MapSpan.FromCenterAndRadius(ViewModel.CurrentLocation, Distance.FromMeters(100)));
            }

            // If odutelep is selected, add pins for all odus and move the map to show all pins
            if (e.PropertyName == nameof(ViewModel.OduList) 
                && ViewModel.ShowMapRegion is not null 
                && ViewModel.OduList.Count != 0 
                && ViewModel.SelectedOdutelep.Id != 0)
            {
                // Clear up old pins and selection
                OduMap.Pins.Clear();

                foreach (var odu in ViewModel.OduList.Where(odu => odu.GpsLatitude != 0 && odu.GpsLongitude != 0))
                {
                    AddPin((double)odu.GpsLatitude, (double)odu.GpsLongitude, odu.OduAzonosito ?? "", odu.Id);
                }

                OduMap.MoveToRegion(ViewModel.ShowMapRegion);
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
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

        ViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(ViewModel.OduList))
            {                
                MyMap.Pins.Clear();
                if(ViewModel.OduList.Count == 0)
                {
                    return;
                }    

                var firstOdu = ViewModel.OduList.FirstOrDefault(o => o.GpsLatitude != 0 && o.GpsLongitude != 0);
                var maxDistance =0.0;
                foreach (var odu in ViewModel.OduList)
                {
                    if (odu.GpsLatitude != 0 && odu.GpsLongitude != 0)
                    {
                        var distance = firstOdu != null ? Location.CalculateDistance(
                            new Location((double)firstOdu.GpsLatitude, (double)firstOdu.GpsLongitude), 
                            new Location((double)odu.GpsLatitude, (double)odu.GpsLongitude), 
                            DistanceUnits.Kilometers) : 0;

                        if (distance > maxDistance)
                        {
                            maxDistance = distance;
                        }

                        AddPin((double)odu.GpsLatitude, (double)odu.GpsLongitude, odu.OduAzonosito ?? "");
                    }
                }
                
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Location((double)firstOdu.GpsLatitude, (double)firstOdu.GpsLongitude), 
                    Distance.FromKilometers(maxDistance)));       
            }
        };
    }

    private void AddPin(double latitude, double longitude, string label)
    {
        var pin = new Pin
        {
            Location = new Location(latitude, longitude),
            Label = label,
            Type = PinType.Place
        };
        MyMap.Pins.Add(pin);
    }

}
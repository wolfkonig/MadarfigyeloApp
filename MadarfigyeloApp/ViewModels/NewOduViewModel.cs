using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.API;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Services;

namespace MadarfigyeloApp.ViewModels
{
    public class NewOduViewModel : BaseViewModel
    {
        private readonly IOduApi _oduApi;
        private readonly IOdutelepApi _odutelepApi;

        // Backing fields
        private string? _oduAzonosito;
        private Odutelep? _selectedOdutelep;
        private string? _oduTipus;
        private int _bejaratiNyilasMm;
        private decimal _gpsLatitude;
        private decimal _gpsLongitude;
        private string? _elohelykod;
        private string? _mireVanHelyezve;
        private string? _felhelyezesModja;
        private string? _oduTajolasa;
        private string? _odutTartoNovenyfaj;
        private string? _magassagMeter;
        private List<Odutelep> _odutelepek = new();

        public AsyncRelayCommand SaveCommand { get; set; }

        public NewOduViewModel(IOduApi oduApi, IOdutelepApi odutelepApi, INavigationService navigationService) : base(navigationService)
        {
            _oduApi = oduApi ?? throw new ArgumentNullException(nameof(oduApi));
            _odutelepApi = odutelepApi ?? throw new ArgumentNullException(nameof(odutelepApi));
            SaveCommand = new(SaveAsync);
        }

        public override async Task InitAsync()
        {
            Odutelepek = await _odutelepApi.GetAllOdutelepAsync();
        }

        public List<Odutelep> Odutelepek 
        { 
            get => _odutelepek; 
            private set => SetProperty(ref _odutelepek, value);
        }

        public string? OduAzonosito
        {
            get => _oduAzonosito;
            set => SetProperty(ref _oduAzonosito, value);
        }

        public Odutelep? SelectedOdutelep
        {
            get => _selectedOdutelep;
            set => SetProperty(ref _selectedOdutelep, value);
        }

        public string? OduTipus
        {
            get => _oduTipus;
            set => SetProperty(ref _oduTipus, value);
        }

        public int BejaratiNyilasMm
        {
            get => _bejaratiNyilasMm;
            set => SetProperty(ref _bejaratiNyilasMm, value);
        }

        public decimal GpsLatitude
        {
            get => _gpsLatitude;
            set => SetProperty(ref _gpsLatitude, value);
        }

        public decimal GpsLongitude
        {
            get => _gpsLongitude;
            set => SetProperty(ref _gpsLongitude, value);
        }

        public string? Elohelykod
        {
            get => _elohelykod;
            set => SetProperty(ref _elohelykod, value);
        }

        public string? MireVanHelyezve
        {
            get => _mireVanHelyezve;
            set => SetProperty(ref _mireVanHelyezve, value);
        }

        public string? FelhelyezesModja
        {
            get => _felhelyezesModja;
            set => SetProperty(ref _felhelyezesModja, value);
        }

        public string? OduTajolasa
        {
            get => _oduTajolasa;
            set => SetProperty(ref _oduTajolasa, value);
        }

        public string? OdutTartoNovenyfaj
        {
            get => _odutTartoNovenyfaj;
            set => SetProperty(ref _odutTartoNovenyfaj, value);
        }

        public string? MagassagMeter
        {
            get => _magassagMeter;
            set => SetProperty(ref _magassagMeter, value);
        }

        private async Task SaveAsync()
        {
            if (SelectedOdutelep == null)
            {
                await _navigationService.ShowAlert("Kérjük, válasszon egy odútelepet!");
                return;
            }

            // Logic to save the Odu linked to SelectedOdutelep.Id
            await _navigationService.ShowAlert("Az odú adatai mentve.");
        }
    }
}

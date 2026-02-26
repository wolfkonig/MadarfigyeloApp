using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.ViewModels
{
    public class NewOduViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly ILocationService _locationService;
        private readonly ILoggerService _logger;

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

        public NewOduViewModel(
            IApiService apiService, 
            INavigationService navigationService, 
            ILocationService locationService,
            ILoggerService logger) : base(navigationService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            SaveCommand = new(SaveAsync);
        }

        public override async Task InitAsync()
        {
            Odutelepek = await _apiService.GetAllOdutelepAsync();
            Location? location = null;
            try
            {
                location = await _locationService.GetCurrentLocationAsync(highAccuracy: true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get current location. ERROR: {ex.Message}");

                var retry = await _navigationService.ShowQuestionAsync("Failed to get current location.","Do you want to retry?");
                if (retry)
                {
                    location = await _locationService.GetCurrentLocationAsync(highAccuracy: false);
                }
            }

            if (location is not null)
            {
                GpsLatitude = (decimal)location.Latitude;
                GpsLongitude = (decimal)location.Longitude;

                //var message = await Utilities.GeocodingHelpers.GetAddressAsync(location);
                //await _navigationService.ShowAlertAsync(message);
            }
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
            if (await Validate())
            {
                var odu = new Odu
                {
                    OduAzonosito = OduAzonosito,
                    OdutelepId = SelectedOdutelep.Id,
                    OduTipus = OduTipus,
                    BejaratiNyilasMm = BejaratiNyilasMm,
                    GpsLatitude = GpsLatitude,
                    GpsLongitude = GpsLongitude,
                    Elohelykod = Elohelykod,
                    MireVanHelyezve = MireVanHelyezve,
                    FelhelyezesModja = FelhelyezesModja,
                    OduTajolasa = OduTajolasa,
                    OdutTartoNovenyfaj = OdutTartoNovenyfaj,
                    MagassagMeter = MagassagMeter
                };

                var success = await _apiService.PostOduAsync(odu);

                if (success)
                {
                    await _navigationService.ShowAlertAsync(string.Format(Resources.AppRes.SaveSuccessful, Resources.AppRes.Odu));
                    await _navigationService.PopAsync();
                }
            }
        }

        private async Task<bool> Validate()
        {
            _errors.Clear();
            if (string.IsNullOrEmpty(OduAzonosito)) _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(OduAzonosito)));
            if (string.IsNullOrEmpty(OduTipus)) _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(OduTipus)));
            if (BejaratiNyilasMm <= 0) _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(BejaratiNyilasMm)));
            if (GpsLatitude <= 0) _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(GpsLatitude)));
            if (GpsLongitude <= 0) _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(GpsLongitude)));
            if (SelectedOdutelep is null) _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(Odutelep)));

            if (HasErrors)
            {
                var message = _errors.Aggregate((a, b) => $"{a}\r\n{b}");
                await _navigationService.ShowAlertAsync(message);
                return false;
            }
            return true;
        }
    }
}

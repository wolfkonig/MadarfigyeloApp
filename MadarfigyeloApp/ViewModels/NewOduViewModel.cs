using CommunityToolkit.Mvvm.Input;
using Terepnaplo.Resources;
using Terepnaplo.Contracts;
using Terepnaplo.Models;

namespace Terepnaplo.ViewModels
{
    public class NewOduViewModel : BaseViewModel
    {
        protected readonly IOduApiService _oduApi;
        protected readonly IOdutelepApiService _odutelepApi;
        protected readonly ILocationService _locationService;
        protected readonly ILoggerService _logger;
        protected readonly ISettingsService _settingsService;

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

        public virtual AsyncRelayCommand SaveCommand { get; set; }

        public NewOduViewModel(
            IOduApiService apiService, 
            IOdutelepApiService odutelepApiService,
            INavigationService navigationService, 
            ILocationService locationService,
            ISettingsService settingsService,
            ILoggerService logger) : base(navigationService)
        {
            _oduApi = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _odutelepApi = odutelepApiService ?? throw new ArgumentNullException(nameof(odutelepApiService));
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            SaveCommand = new(SaveAsync);
        }

        public override async Task InitAsync()
        {
            Odutelepek = await _odutelepApi.GetAllOdutelepAsync();
            SelectedOdutelep = Odutelepek.SingleOrDefault(x => x.Id == _settingsService.SelectedOdutelepId);
            await GetLocation();
        }

        private async Task GetLocation()
        {
            Location? location = null;
            LoadingMessage = AppRes.LocationLoading;
            IsBusy = true;

            try
            {
                location = await _locationService.GetCurrentLocationAsync(highAccuracy: true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get current location. ERROR: {ex.Message}");
            }
            finally
            {
                LoadingMessage = string.Empty;
                IsBusy = false;
            }

            var retry = false;
            if (location is null || location.Latitude == 0 || location.Longitude == 0)
            {
                retry = await _navigationService.ShowQuestionAsync(AppRes.Location, AppRes.LocationErrorRetry, AppRes.Yes, AppRes.No);
            }
            else if (location.Accuracy > Constants.LocationDesiredAccuracyMeters)
            {
                var message = string.Format(AppRes.LocationAccuracyError, Math.Round((decimal)location.Accuracy, 2));
                retry = await _navigationService.ShowQuestionAsync(AppRes.Location, message, AppRes.Yes, AppRes.No);
            }

            if (retry)
            {
                await GetLocation();
            }
            else if (location is not null)
            {
                GpsLatitude = (decimal)location.Latitude;
                GpsLongitude = (decimal)location.Longitude;
            }
        }

        public virtual string PageTitle => AppRes.UjOdu;

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
            set
            {
                SetProperty(ref _selectedOdutelep, value);
                _settingsService.SelectedOdutelepId = value?.Id ?? 0;
            }
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

        protected virtual async Task SaveAsync()
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

                var success = await _oduApi.PostOduAsync(odu);

                if (success)
                {
                    await _navigationService.ShowAlertAsync(string.Format(AppRes.SaveSuccessful, AppRes.Odu));
                    await _navigationService.PopAsync();
                }
            }
        }

        protected async Task<bool> Validate()
        {
            _errors.Clear();
            if (string.IsNullOrEmpty(OduAzonosito)) _errors.Add(string.Format(AppRes.ErrorEmpty, nameof(OduAzonosito)));
            if (string.IsNullOrEmpty(OduTipus)) _errors.Add(string.Format(AppRes.ErrorEmpty, nameof(OduTipus)));
            if (BejaratiNyilasMm <= 0) _errors.Add(string.Format(AppRes.ErrorEmpty, nameof(BejaratiNyilasMm)));
            if (GpsLatitude == 0) _errors.Add(string.Format(AppRes.ErrorEmpty, nameof(GpsLatitude)));
            if (GpsLongitude == 0) _errors.Add(string.Format(AppRes.ErrorEmpty, nameof(GpsLongitude)));
            if (SelectedOdutelep is null || SelectedOdutelep.Id == 0) _errors.Add(string.Format(AppRes.ErrorEmpty, nameof(Odutelep)));

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

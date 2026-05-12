using CommunityToolkit.Mvvm.Input;
using Terepnaplo.Contracts;
using Terepnaplo.Models;

namespace Terepnaplo.ViewModels
{
    public class NewLatogatasViewModel : BaseViewModel
    {
        protected readonly IOduApiService _oduApi;
        protected readonly ILatogatasApiService _latogatasApi;
        protected readonly ISettingsService _settingsService;

        // Backing fields
        private Odu? _selectedOdu;
        private DateTime _datum = DateTime.Now;
        private Tevekenyseg? _tevekenyseg;
        private Allapot? _allapot;
        private string? _faj;
        private int _tojasSzam;
        private int _fiokaSzam;
        private string? _fiokakKora;
        private string? _megjegyzesek;
        private List<Odu> _oduk = new();

        public NewLatogatasViewModel(IOduApiService oduApiService, ILatogatasApiService latogatasApiService, INavigationService navigationService, ISettingsService settingsService ) : base(navigationService)
        {
            _oduApi = oduApiService ?? throw new ArgumentNullException(nameof(oduApiService));
            _latogatasApi = latogatasApiService ?? throw new ArgumentNullException(nameof(latogatasApiService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            SaveCommand = new(SaveAsync);
        }

        public virtual AsyncRelayCommand SaveCommand { get; protected set; }

        public virtual string PageTitle => Resources.AppRes.UjLatogatas;

        public override async Task InitAsync()
        {
            Oduk = await _oduApi.GetAllOduAsync();
            SelectedOdu = Oduk.FirstOrDefault(x => x.Id == _settingsService.SelectedOduId);
        }

        // Collections for Pickers
        public List<Odu> Oduk
        {
            get => _oduk;
            private set => SetProperty(ref _oduk, value);
        }

        public List<Tevekenyseg> Tevekenysegek { get; } = [.. Enum.GetValues<Tevekenyseg>()];

        public List<Allapot> Allapotok { get; } = [.. Enum.GetValues<Allapot>()];

        public Odu? SelectedOdu
        {
            get => _selectedOdu;
            set
            {
                SetProperty(ref _selectedOdu, value);
                _settingsService.SelectedOduId = value?.Id ?? 0;
            }
        }

        public DateTime Datum
        {
            get => _datum;
            set => SetProperty(ref _datum, value);
        }

        public Tevekenyseg? Tevekenyseg
        {
            get => _tevekenyseg;
            set => SetProperty(ref _tevekenyseg, value);
        }

        public Allapot? Allapot
        {
            get => _allapot;
            set => SetProperty(ref _allapot, value);
        }

        public string? Faj
        {
            get => _faj;
            set => SetProperty(ref _faj, value);
        }

        public int TojasSzam
        {
            get => _tojasSzam;
            set => SetProperty(ref _tojasSzam, value);
        }

        public int FiokaSzam
        {
            get => _fiokaSzam;
            set => SetProperty(ref _fiokaSzam, value);
        }

        public string? FiokakKora
        {
            get => _fiokakKora;
            set => SetProperty(ref _fiokakKora, value);
        }

        public string? Megjegyzesek
        {
            get => _megjegyzesek;
            set => SetProperty(ref _megjegyzesek, value);
        }

        protected virtual async Task SaveAsync()
        {
            if (await Validate())
            {
                var ltg = new Latogatas
                {
                    OduId = SelectedOdu.Id,
                    Datum = Datum,
                    Tevekenyseg = Tevekenyseg.Value,
                    Allapot = Allapot.Value,
                    Faj = Faj ?? string.Empty,
                    TojasSzam = TojasSzam,
                    FiokaSzam = FiokaSzam,
                    FiokakKora = FiokakKora,
                    Megjegyzesek = Megjegyzesek
                };

                var success = await _latogatasApi.PostLatogatasAsync(ltg);

                if (success)
                {
                    await _navigationService.ShowAlertAsync(string.Format(Resources.AppRes.SaveSuccessful, Resources.AppRes.Latogatas));
                    await _navigationService.PopAsync();
                }
            }
        }

        protected async Task<bool> Validate()
        {
            _errors.Clear();
            if (SelectedOdu is null) _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(Odu)));
            if (Tevekenyseg is null) _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(Tevekenyseg)));
            if (Allapot is null) _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(Allapot)));  

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

using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.API;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Implementations;
using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.ViewModels
{
    public class NewLatogatasViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        // Backing fields
        private Odu? _selectedOdu;
        private DateTime _datum = DateTime.Now;
        private TevekenysegModel? _tevekenyseg;
        private AllapotModel? _allapot;
        private string? _faj;
        private int _tojasSzam;
        private int _fiokaSzam;
        private string? _fiokakKora;
        private string? _megjegyzesek;
        private List<Odu> _oduk = new();

        public NewLatogatasViewModel(IApiService apiService, INavigationService navigationService) : base(navigationService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
             SaveCommand = new(SaveAsync);
        }

        public AsyncRelayCommand SaveCommand { get; }

        public override async Task InitAsync()
        {
            Oduk = await _apiService.GetAllOduAsync();
        }

        // Collections for Pickers
        public List<Odu> Oduk
        {
            get => _oduk;
            private set => SetProperty(ref _oduk, value);
        }

        public List<TevekenysegModel> Tevekenysegek { get; } = Enum.GetValues<Tevekenyseg>()
            .Select(t => new TevekenysegModel(t))
            .ToList();

        public List<AllapotModel> Allapotok { get; } = Enum.GetValues<Allapot>()            
            .Select(a=>new AllapotModel(a))
            .ToList();

        public Odu? SelectedOdu
        {
            get => _selectedOdu;
            set => SetProperty(ref _selectedOdu, value);
        }

        public DateTime Datum
        {
            get => _datum;
            set => SetProperty(ref _datum, value);
        }

        public TevekenysegModel Tevekenyseg
        {
            get => _tevekenyseg;
            set => SetProperty(ref _tevekenyseg, value);
        }

        public AllapotModel Allapot
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

        public async Task SaveAsync()
        {
            if (await Validate())
            {
                var ltg = new Latogatas
                {
                    OduId = SelectedOdu.Id,
                    Datum = Datum,
                    Tevekenyseg = Tevekenyseg.Value,
                    Allapot = Allapot.Value,
                    Faj = Faj,
                    TojasSzam = TojasSzam,
                    FiokaSzam = FiokaSzam,
                    FiokakKora = FiokakKora,
                    Megjegyzesek = Megjegyzesek
                };

                var success = await _apiService.PostLatogatasAsync(ltg);

                if (success)
                {
                    await _navigationService.ShowAlertAsync(string.Format(Resources.AppRes.SaveSuccessful, Resources.AppRes.Latogatas));
                    await _navigationService.PopAsync();
                }
            }
        }

        private async Task<bool> Validate()
        {
            _errors.Clear();
            if (SelectedOdu is null) _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(Odu)));
            if (Tevekenyseg is null) _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(Tevekenyseg)));
            if (Allapot is null) _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(Allapot)));
            if (string.IsNullOrEmpty(Faj)) _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(Faj)));           

            if (_errors.Count > 0)
            {
                var message = _errors.Aggregate((a, b) => $"{a}\r\n{b}");
                await _navigationService.ShowAlertAsync(message);
                return false;
            }
            return true;
        }
    }
}

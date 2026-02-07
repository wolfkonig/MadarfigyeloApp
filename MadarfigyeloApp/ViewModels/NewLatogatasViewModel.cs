using MadarfigyeloApp.API;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Services;

namespace MadarfigyeloApp.ViewModels
{
    public class NewLatogatasViewModel : BaseViewModel
    {
        private readonly ILatogatasApi _latogatasApi;
        private readonly IOduApi _oduApi;

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

        public NewLatogatasViewModel(ILatogatasApi latogatasApi, IOduApi oduApi, INavigationService navigationService) : base(navigationService)
        {
            _latogatasApi = latogatasApi ?? throw new ArgumentNullException(nameof(latogatasApi));
            _oduApi = oduApi ?? throw new ArgumentNullException(nameof(oduApi));
        }

        public override async Task InitAsync()
        {
            Oduk = await _oduApi.GetAllOduAsync();
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

        private async Task Save()
        {
            if (SelectedOdu == null)
            {
                await _navigationService.ShowAlert("Kérjük, válasszon egy odút!");
                return;
            }
            // Logic to save the Latogatas record
            await _navigationService.ShowAlert("Látogatás adatai mentve!");
        }
    }
}

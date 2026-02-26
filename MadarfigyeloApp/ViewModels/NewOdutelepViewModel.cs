using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.ViewModels
{
    public class NewOdutelepViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        // Backing fields
        private string? _azonosito;
        private string? _telepules;
        private string? _teruletNev;
        private string? _utmNegyzetKod;
        private string? _kezeloSzervezetNev;
        private string? _felelosSzemelyNev;
        private string? _felelosSzemelyCim;
        private string? _felelosSzemelyTelefonszam;
        private string? _felelosSzemelyEmail;
        private string? _megjegyzes;

        public NewOdutelepViewModel(IApiService apiService, INavigationService navigationService) : base(navigationService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            SaveCommand = new(SaveAsync);
        }

        public AsyncRelayCommand SaveCommand { get; }

        // Full properties with SetProperty
        public string? Azonosito
        {
            get => _azonosito;
            set => SetProperty(ref _azonosito, value);
        }

        public string? Telepules
        {
            get => _telepules;
            set => SetProperty(ref _telepules, value);
        }

        public string? TeruletNev
        {
            get => _teruletNev;
            set => SetProperty(ref _teruletNev, value);
        }

        public string? UtmNegyzetKod
        {
            get => _utmNegyzetKod;
            set => SetProperty(ref _utmNegyzetKod, value);
        }

        public string? KezeloSzervezetNev
        {
            get => _kezeloSzervezetNev;
            set => SetProperty(ref _kezeloSzervezetNev, value);
        }

        public string? FelelosSzemelyNev
        {
            get => _felelosSzemelyNev;
            set => SetProperty(ref _felelosSzemelyNev, value);
        }

        public string? FelelosSzemelyCim
        {
            get => _felelosSzemelyCim;
            set => SetProperty(ref _felelosSzemelyCim, value);
        }

        public string? FelelosSzemelyTelefonszam
        {
            get => _felelosSzemelyTelefonszam;
            set => SetProperty(ref _felelosSzemelyTelefonszam, value);
        }

        public string? FelelosSzemelyEmail
        {
            get => _felelosSzemelyEmail;
            set => SetProperty(ref _felelosSzemelyEmail, value);
        }

        public string? Megjegyzes
        {
            get => _megjegyzes;
            set => SetProperty(ref _megjegyzes, value);
        }

        public override Task InitAsync()
        {
           return Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            if (await Validate())
            {
                var ot = new Odutelep
                {
                    TeruletNev = TeruletNev, //NN
                    Telepules = Telepules, //NN
                    Megjegyzes = Megjegyzes,
                    KezeloSzervezetNev = KezeloSzervezetNev,
                    Azonosito = Azonosito, //NN
                    FelelosSzemelyCim = FelelosSzemelyCim,
                    FelelosSzemelyEmail = FelelosSzemelyEmail,
                    FelelosSzemelyTelefonszam = FelelosSzemelyTelefonszam,
                    UtmNegyzetKod = UtmNegyzetKod //NN
                };

               var success = await _apiService.PostOdutelepAsync(ot);

                if (success)
                {
                    await _navigationService.ShowAlertAsync(string.Format(Resources.AppRes.SaveSuccessful, Resources.AppRes.Odutelep));
                    await _navigationService.PopAsync();
                }
            }
        }

        private async Task<bool> Validate()
        {
            _errors.Clear();
            if(string.IsNullOrEmpty(Azonosito))
            {
                _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(Azonosito)));                
            }
            if (string.IsNullOrEmpty(TeruletNev))
            {
                _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(TeruletNev)));
            }
            if (string.IsNullOrEmpty(Telepules))
            {
                _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(Telepules)));
            }
            if (string.IsNullOrEmpty(UtmNegyzetKod))
            {
                _errors.Add(string.Format(Resources.AppRes.ErrorEmpty, nameof(UtmNegyzetKod)));
            }

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

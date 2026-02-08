using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.API;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.ViewModels
{
    public class NewOdutelepViewModel : BaseViewModel
    {
        private readonly IOdutelepApi _odutelepApi;

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

        public NewOdutelepViewModel(INavigationService navigationService, IOdutelepApi odutelepApi) : base(navigationService)
        {
            _odutelepApi = odutelepApi ?? throw new ArgumentNullException(nameof(odutelepApi));
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

                await _odutelepApi.PostOdutelepAsync(ot);
                await _navigationService.PopAsync();

                await _navigationService.ShowAlert(string.Format(Resources.AppRes.SaveSuccessful, Resources.AppRes.Odutelep));
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

            if (_errors.Count > 0)
            {
                var message = _errors.Aggregate((a, b) => $"{a}\r\n{b}");
                await _navigationService.ShowAlert(message);
                return false;
            }
            return true;
        }

    }
}

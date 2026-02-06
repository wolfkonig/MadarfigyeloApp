using MadarfigyeloApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadarfigyeloApp.ViewModels
{
    public class NewOdutelepViewModel : BaseViewModel
    {
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

        public NewOdutelepViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

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
    }
}

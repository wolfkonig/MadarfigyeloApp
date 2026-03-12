using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadarfigyeloApp.ViewModels
{
    public class OduMapViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly ILocationService _locationService;
        private readonly ISettingsService _settingsService;

        private List<Odu> _oduList = [];
        private List<Odutelep> _odutelepList = [Odutelep.Empty];
        private Odutelep _selectedOdutelep;
        private bool _oduSelected;

        public List<Odu> OduList
        {
            get => _oduList;
            set => SetProperty(ref _oduList, value);
        }

        public List<Odutelep> OdutelepList
        {
            get => _odutelepList;
            set => SetProperty(ref _odutelepList, value);
        }

        public Odutelep SelectedOdutelep
        {
            get => _selectedOdutelep;
            set
            {
                _settingsService.SelectedOdutelepId = value?.Id ?? 0;
                SetProperty(ref _selectedOdutelep, value);
                OnPropertyChanged(nameof(OduList));
            }
        }

        public bool OduSelected 
        { 
            get => _oduSelected; 
            set => _oduSelected = value; 
        }

        public int SelectedOduId { get; set; }

        public AsyncRelayCommand NewOduCommand { get; private set; }
        public AsyncRelayCommand LatogatasokCommand { get; private set; }

        public OduMapViewModel(
            IApiService apiService, 
            INavigationService navigationService, 
            ILocationService locationService,
            ISettingsService settingsService) : base(navigationService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            LatogatasokCommand = new(LatogatasokAsync);

            PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedOdutelep) && !IsBusy)
                {
                    IsBusy = true;
                    await PopulateOduList()
                    .ContinueWith(t => IsBusy = false);
                }
            };
        }

        public override async Task InitAsync()
        {
            await Task.WhenAll(
                PopulateOdutelepDropdown(),
                PopulateOduList()
            );
        }

        private async Task PopulateOduList()
        {
            if (_settingsService.SelectedOdutelepId == 0)
            {
                OduList = await _apiService.GetAllOduAsync();
            }
            else
            {
                OduList = await _apiService.GetOduByOdutelepAsync(_settingsService.SelectedOdutelepId);
            }
        }

        private async Task PopulateOdutelepDropdown()
        {
            var odutelepek = await _apiService.GetAllOdutelepAsync();
            if (_odutelepList.Count == 1)
            {
                OdutelepList = [Odutelep.Empty, .. odutelepek];
            }
            SelectedOdutelep = OdutelepList.SingleOrDefault(x => x.Id == _settingsService.SelectedOdutelepId) ?? OdutelepList[0];
        }

        private async Task LatogatasokAsync()
        {
            _settingsService.SelectedOduId = SelectedOduId;
            await _navigationService.GoToAsync($"//{Constants.RouteLatogatasok}");
        }
    }
}

using CommunityToolkit.Mvvm.Input;
using Terepnaplo.Contracts;
using Terepnaplo.Models;

namespace Terepnaplo.ViewModels
{
    public class OdutelepViewModel : BaseViewModel
    {
        private readonly IOdutelepApiService _odutelepApiService;
        private readonly ISettingsService _settingsService;

        private List<Odutelep> _odutelepList = [];
        private bool _isRefreshing;

        public List<Odutelep> OdutelepList
        {
            get => _odutelepList;
            set => SetProperty(ref _odutelepList, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public AsyncRelayCommand NewOdutelepCommand { get; }
        public AsyncRelayCommand<int> OdukCommand { get; }
        public AsyncRelayCommand<int> OduMapCommand { get; }
        public AsyncRelayCommand RefreshCommand { get; }

        public OdutelepViewModel(IOdutelepApiService odutelepApiService, INavigationService navigationService, ISettingsService settingsService) : base(navigationService)
        {
            _odutelepApiService = odutelepApiService ?? throw new ArgumentNullException(nameof(odutelepApiService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            NewOdutelepCommand = new(NewOdutelep);
            OdukCommand = new(OdukAsync);
            RefreshCommand = new(RefreshAsync); 
            OduMapCommand = new(OduMapAsync);
        }

        public override async Task InitAsync()
        {
            OdutelepList = await _odutelepApiService.GetAllOdutelepAsync();
        }

        protected async Task RefreshAsync()
        {
            IsRefreshing = true;
            try
            {
                OdutelepList = await _odutelepApiService.GetAllOdutelepAsync(forceRefresh: true);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task NewOdutelep()
        {
            await _navigationService.GoToAsync(Constants.RouteNewOdutelep);
        }

        private async Task OdukAsync(int odutelepId)
        {
            _settingsService.SelectedOdutelepId = odutelepId;
            await _navigationService.GoToAsync($"//{Constants.RouteOduk}");
        }

        private async Task OduMapAsync(int odutelepId)
        {
            _settingsService.SelectedOdutelepId = odutelepId;
            await _navigationService.GoToAsync($"//{Constants.RouteOduMap}");
        }
    }
}

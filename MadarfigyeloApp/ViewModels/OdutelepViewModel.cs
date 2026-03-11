using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.ViewModels
{
    public class OdutelepViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
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

        public AsyncRelayCommand NewOdutelepCommand { get; private set; }
        public AsyncRelayCommand<int> OdukCommand { get; private set; }
        public AsyncRelayCommand RefreshCommand { get; private set; }

        public OdutelepViewModel(IApiService apiService, INavigationService navigationService, ISettingsService settingsService) : base(navigationService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            NewOdutelepCommand = new(NewOdutelep);
            OdukCommand = new(OdukAsync);
            RefreshCommand = new(RefreshAsync);
        }

        public override async Task InitAsync()
        {
            OdutelepList = await _apiService.GetAllOdutelepAsync();
        }

        protected async Task RefreshAsync()
        {
            IsRefreshing = true;
            try
            {
                OdutelepList = await _apiService.GetAllOdutelepAsync(forceRefresh: true);
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
    }
}

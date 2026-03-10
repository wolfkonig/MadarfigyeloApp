using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Resources;

namespace MadarfigyeloApp.ViewModels
{
    public class OduViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly ISettingsService _settingsService;

        private List<Odu> _oduList = [];
        private List<Odutelep> _odutelepList = [Odutelep.Empty];
        private Odutelep _selectedOdutelep;
        private bool _isRefreshing;

        public List<Odu> OduList
        {
            get => [.. _oduList.Where(o => SelectedOdutelep == null || SelectedOdutelep.Id == 0 || o.OdutelepId == SelectedOdutelep.Id)];
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
                if (SetProperty(ref _selectedOdutelep, value))
                {
                    _settingsService.SelectedOdutelepId = value?.Id ?? 0;
                    OnPropertyChanged(nameof(OduList));
                }
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public AsyncRelayCommand NewOduCommand { get; private set; }
        public AsyncRelayCommand<int> LatogatasokCommand { get; private set; }
        public AsyncRelayCommand RefreshCommand { get; private set; }

        public OduViewModel(IApiService apiService, INavigationService navigationService, ISettingsService settingsService) : base(navigationService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            NewOduCommand = new(NewOduAsync);
            LatogatasokCommand = new(LatogatasokAsync);
            RefreshCommand = new(RefreshAsync);
        }

        public override async Task InitAsync()
        {
            await LoadAsync(forceRefresh: false);
        }

        protected async Task RefreshAsync()
        {
            IsRefreshing = true;
            try
            {
                await LoadAsync(forceRefresh: true);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task LoadAsync(bool forceRefresh)
        {            
            var odutelepek = await _apiService.GetAllOdutelepAsync(forceRefresh);
            // Only refresh the list if we have new or deleted items or if we explicitly want to refresh.
            if (_odutelepList.Count == 1 || _odutelepList.Count != odutelepek.Count + 1 || forceRefresh)
            {
                OdutelepList = [.. odutelepek.Concat([Odutelep.Empty]).OrderBy(x => x.Id)];
            }
            SelectedOdutelep = OdutelepList
                .SingleOrDefault(x => x.Id == _settingsService.SelectedOdutelepId) ?? OdutelepList[0];

            var oduk = await _apiService.GetAllOduAsync(forceRefresh);
            foreach (var odu in oduk)
            {
                odu.Odutelep = _odutelepList.FirstOrDefault(x => x.Id == odu.OdutelepId);
            }
            OduList = oduk;
        }

        private async Task NewOduAsync()
        {
            await _navigationService.GoToAsync(Constants.RouteNewOdu);
        }

        private async Task LatogatasokAsync(int oduId)
        {
            _settingsService.SelectedOduId = oduId;
            await _navigationService.GoToAsync($"//{Constants.RouteLatogatasok}");
        }
    }
}

using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Resources;

namespace MadarfigyeloApp.ViewModels
{
    public class LatogatasViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly ISettingsService _settingsService;

        private List<Latogatas> _latogatasList = [];
        private List<Odu> _oduList = [Odu.Empty];
        private Odu _selectedOdu;
        private bool _isRefreshing;

        public List<Latogatas> LatogatasList
        {
            get => [.. _latogatasList.Where(l => SelectedOdu is null || SelectedOdu.Id == 0 || l.OduId == SelectedOdu.Id)];
            set => SetProperty(ref _latogatasList, value);
        }

        public List<Odu> OduList
        {
            get => _oduList;
            set => SetProperty(ref _oduList, value);
        }

        public Odu SelectedOdu
        {
            get => _selectedOdu;
            set
            {
                SetProperty(ref _selectedOdu, value);
                _settingsService.SelectedOduId = value?.Id ?? 0;
                OnPropertyChanged(nameof(LatogatasList));
            }
        }
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public AsyncRelayCommand NewLatogatasCommand { get; private set; }
        public AsyncRelayCommand RefreshCommand { get; private set; }

        public LatogatasViewModel(IApiService apiService, INavigationService navigationService, ISettingsService settingsService) : base(navigationService)
        {            
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            NewLatogatasCommand = new(NewLatogatasAsync);
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

        private async Task LoadAsync(bool forceRefresh = false)
        {
            var oduk = await _apiService.GetAllOduAsync(forceRefresh);
            OduList = [.. oduk.Concat([Odu.Empty]).OrderBy(x => x.Id)];
            SelectedOdu = OduList.FirstOrDefault(x => x.Id == _settingsService.SelectedOduId) ?? OduList[0];

            var latogatasok = await _apiService.GetAllLatogatasAsync(forceRefresh);
            foreach (var latogatas in latogatasok)
            {
                latogatas.Odu = _oduList.FirstOrDefault(x => x.Id == latogatas.OduId);
            }
            LatogatasList = latogatasok;
        }

        private async Task NewLatogatasAsync()
        {
            await _navigationService.GoToAsync(Constants.RouteNewLatogatas);            
        }
    }
}

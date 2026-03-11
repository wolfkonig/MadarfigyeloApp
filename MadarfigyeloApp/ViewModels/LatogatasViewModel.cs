using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;


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
            get => _latogatasList;
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
                _settingsService.SelectedOduId = value?.Id ?? 0;
                SetProperty(ref _selectedOdu, value);
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

            PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedOdu) && !IsRefreshing && !IsBusy)
                {
                    IsBusy = true;
                    await PopulateLatogatasList(forceRefresh: false)
                        .ContinueWith(t => IsBusy = false);
                }
            };
        }

        public override async Task InitAsync()
        {
            await Task.WhenAll(
                PopulateOduDropdown(forceRefresh: false),
                PopulateLatogatasList(forceRefresh: false)
            );
        }

        protected async Task RefreshAsync()
        {
            IsRefreshing = true;
            await Task.WhenAll(
                PopulateOduDropdown(forceRefresh: true),
                PopulateLatogatasList(forceRefresh: true)
            ).ContinueWith(t => IsRefreshing = false);
        }

        private async Task PopulateLatogatasList(bool forceRefresh = false)
        {
            if(_settingsService.SelectedOduId == 0)
            {
                LatogatasList = await _apiService.GetAllLatogatasAsync(forceRefresh);
            }
            else
            {
                LatogatasList = await _apiService.GetLatogatasByOduAsync(_settingsService.SelectedOduId, forceRefresh);
            }            
        }

        private async Task PopulateOduDropdown(bool forceRefresh = false)
        {
            var oduk = await _apiService.GetAllOduAsync(forceRefresh);
            if (_oduList.Count == 1 || forceRefresh)
            {
                OduList = [Odu.Empty, .. oduk];
            }
            SelectedOdu = OduList.FirstOrDefault(x => x.Id == _settingsService.SelectedOduId) ?? OduList[0];
        }

        private async Task NewLatogatasAsync()
        {
            await _navigationService.GoToAsync(Constants.RouteNewLatogatas);            
        }
    }
}

using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;


namespace MadarfigyeloApp.ViewModels
{
    public class LatogatasViewModel : BaseViewModel
    {
        private readonly ILatogatasApiService _latogatasApi;
        private readonly IOduApiService _oduApi;
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
                if (!IsBusy)
                {
                    // Should not set SelectedOduId during init
                    _settingsService.SelectedOduId = value?.Id ?? 0;
                }
                SetProperty(ref _selectedOdu, value);
                OnPropertyChanged(nameof(LatogatasList));
                OnPropertyChanged(nameof(SelectedOdutelep));
            }
        }

        public Odutelep SelectedOdutelep => SelectedOdu?.Odutelep ?? Odutelep.Empty;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public AsyncRelayCommand NewLatogatasCommand { get; private set; }
        public AsyncRelayCommand RefreshCommand { get; private set; }
        public AsyncRelayCommand<int> EditOduCommand { get; private set; }

        public LatogatasViewModel(IOduApiService oduApiService, ILatogatasApiService latogatasApiService, INavigationService navigationService, ISettingsService settingsService) : base(navigationService)
        {            
            _oduApi = oduApiService ?? throw new ArgumentNullException(nameof(oduApiService));
            _latogatasApi = latogatasApiService ?? throw new ArgumentNullException(nameof(latogatasApiService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

            NewLatogatasCommand = new(NewLatogatasAsync);
            RefreshCommand = new(RefreshAsync);
            EditOduCommand = new(EditOduAsync);

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
                PopulateOduDropdown(),
                PopulateLatogatasList()
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
                LatogatasList = await _latogatasApi.GetAllLatogatasAsync(forceRefresh);
            }
            else
            {
                LatogatasList = await _latogatasApi.GetLatogatasByOduAsync(_settingsService.SelectedOduId, forceRefresh);
            }            
        }

        private async Task PopulateOduDropdown(bool forceRefresh = false)
        {
            List<Odu> oduList;
            if (_settingsService.SelectedOdutelepId == 0)
            {
                oduList = await _oduApi.GetAllOduAsync(forceRefresh);
            }
            else
            {
                oduList = await _oduApi.GetOduByOdutelepAsync(_settingsService.SelectedOdutelepId, forceRefresh);
            }

            OduList = [Odu.Empty, .. oduList];
            SelectedOdu = OduList.FirstOrDefault(x => x.Id == _settingsService.SelectedOduId) ?? OduList[0];
        }

        private async Task NewLatogatasAsync()
        {
            await _navigationService.GoToAsync(Constants.RouteNewLatogatas);            
        }

        private async Task EditOduAsync(int oduId)
        {
            _settingsService.SelectedOduId = oduId;
            await _navigationService.GoToAsync(Constants.RouteEditOdu);
        }
    }
}

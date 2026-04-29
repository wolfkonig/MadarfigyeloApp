using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Resources;

namespace MadarfigyeloApp.ViewModels
{
    public class OduViewModel : BaseViewModel
    {
        private readonly IOdutelepApiService _odutelepApi;
        private readonly IOduApiService _oduApi;
        private readonly ISettingsService _settingsService;

        private List<Odu> _oduList = [];
        private List<Odutelep> _odutelepList = [Odutelep.Empty];
        private Odutelep _selectedOdutelep;
        private bool _isRefreshing;

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
                if (!IsBusy)
                {
                    // Should not set SelectedOdutelepId during init
                    _settingsService.SelectedOdutelepId = value?.Id ?? 0;
                }
                SetProperty(ref _selectedOdutelep, value);
                OnPropertyChanged(nameof(OduList));
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public AsyncRelayCommand NewOduCommand { get; }
        public AsyncRelayCommand<int> LatogatasokCommand { get; }
        public AsyncRelayCommand<int> EditOduCommand { get; }
        public AsyncRelayCommand<int> DeleteOduCommand { get; }
        public AsyncRelayCommand RefreshCommand { get; }

        public OduViewModel(
            IOdutelepApiService odutelepApiService, 
            IOduApiService oduApiService, 
            INavigationService navigationService, 
            ISettingsService settingsService) : base(navigationService)
        {
            _odutelepApi = odutelepApiService ?? throw new ArgumentNullException(nameof(odutelepApiService));
            _oduApi = oduApiService ?? throw new ArgumentNullException(nameof(oduApiService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

            NewOduCommand = new(NewOduAsync);
            LatogatasokCommand = new(LatogatasokAsync);
            EditOduCommand = new(EditOduAsync);
            RefreshCommand = new(RefreshAsync);
            DeleteOduCommand = new(DeleteOduAsync);

            PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedOdutelep) && !IsRefreshing && !IsBusy)
                {
                    IsBusy = true;
                    await PopulateOduList(forceRefresh: false)
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

        protected async Task RefreshAsync()
        {
            IsRefreshing = true;
            await Task.WhenAll(
                    PopulateOdutelepDropdown(forceRefresh: true),
                    PopulateOduList(forceRefresh: true))
                .ContinueWith(t => IsRefreshing = false);
        }

        private async Task PopulateOduList(bool forceRefresh = false)
        {
            if (_settingsService.SelectedOdutelepId == 0)
            {
                OduList = await _oduApi.GetAllOduAsync(forceRefresh);
            }
            else
            {
                OduList = await _oduApi.GetOduByOdutelepAsync(_settingsService.SelectedOdutelepId, forceRefresh);
            }
        }

        private async Task PopulateOdutelepDropdown(bool forceRefresh = false)
        {
            var odutelepek = await _odutelepApi.GetAllOdutelepAsync(forceRefresh);
            if (_odutelepList.Count == 1 || forceRefresh)
            {
                OdutelepList = [Odutelep.Empty, .. odutelepek];
            }
            SelectedOdutelep = OdutelepList.SingleOrDefault(x => x.Id == _settingsService.SelectedOdutelepId) ?? OdutelepList[0];
        }

        private async Task NewOduAsync()
        {
            await _navigationService.GoToAsync(Constants.RouteNewOdu);
        }

        private async Task EditOduAsync(int oduId)
        {
            _settingsService.SelectedOduId = oduId;
            await _navigationService.GoToAsync(Constants.RouteEditOdu);
        }

        private async Task DeleteOduAsync(int oduId)
        {
            if (!await _navigationService.ShowQuestionAsync(AppRes.DeleteOdu, AppRes.ConfirmDeleteOdu, AppRes.Yes, AppRes.No))
            {
                return;
            }

            var deleted = await _oduApi.DeleteOduAsync(oduId);

            if (deleted)
            {

                if (_settingsService.SelectedOduId == oduId)
                {
                    _settingsService.SelectedOduId = 0;
                }
                IsBusy = true;
                await PopulateOduList(forceRefresh: true)
                    .ContinueWith(_ => IsBusy = false);
            }
            else
            {
                await _navigationService.ShowAlertAsync(AppRes.DeleteFailed);
            }

        }

        private async Task LatogatasokAsync(int oduId)
        {
            _settingsService.SelectedOduId = oduId;
            await _navigationService.GoToAsync($"//{Constants.RouteLatogatasok}");
        }
    }
}

using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Resources;


namespace MadarfigyeloApp.ViewModels
{
    public class LatogatasViewModel : BaseViewModel
    {
        private readonly ILatogatasApiService _latogatasApi;
        private readonly IOduApiService _oduApi;
        private readonly IOdutelepApiService _odutelepApi;
        private readonly ISettingsService _settingsService;

        private List<Latogatas> _latogatasList = [];
        private List<Odu> _oduList = [Odu.Empty];
        private List<Odutelep> _odutelepList = [Odutelep.Empty];
        private Odu _selectedOdu;
        private Odutelep _selectedOdutelep;
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

        public List<Odutelep> OdutelepList
        {
            get => _odutelepList;
            set => SetProperty(ref _odutelepList, value);
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
            }
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
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public AsyncRelayCommand NewLatogatasCommand { get; }
        public AsyncRelayCommand RefreshCommand { get; }
        public AsyncRelayCommand<int> EditOduCommand { get; }
        public AsyncRelayCommand<int> EditLatogatasCommand { get; }
        public AsyncRelayCommand<int> DeleteLatogatasCommand { get; }

        public LatogatasViewModel(
            IOdutelepApiService odutelepApiService,
            IOduApiService oduApiService, 
            ILatogatasApiService latogatasApiService, 
            INavigationService navigationService, 
            ISettingsService settingsService) : base(navigationService)
        {
            _odutelepApi = odutelepApiService ?? throw new ArgumentNullException(nameof(odutelepApiService));
            _oduApi = oduApiService ?? throw new ArgumentNullException(nameof(oduApiService));
            _latogatasApi = latogatasApiService ?? throw new ArgumentNullException(nameof(latogatasApiService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

            NewLatogatasCommand = new(NewLatogatasAsync);
            RefreshCommand = new(RefreshAsync);
            EditOduCommand = new(EditOduAsync);
            EditLatogatasCommand = new(EditLatogatasAsync);
            DeleteLatogatasCommand = new(DeleteLatogatasAsync);

            PropertyChanged += async (s, e) =>
            {
                if(IsRefreshing || IsBusy)
                {
                    return;
                }

                if (e.PropertyName == nameof(SelectedOdu))
                {
                    IsBusy = true;
                    await PopulateLatogatasList(forceRefresh: false)
                        .ContinueWith(t => IsBusy = false);
                }
                else if (e.PropertyName == nameof(SelectedOdutelep))
                {
                    IsBusy = true;
                    // Reset Odu selection when Odutelep changes
                    _settingsService.SelectedOduId = 0;
                    await Task.WhenAll(
                        PopulateOduDropdown(forceRefresh: true),
                        PopulateLatogatasList(forceRefresh: true)
                    ).ContinueWith(t => IsBusy = false);
                }
            };
        }

        public override async Task InitAsync()
        {
            await Task.WhenAll(
                PopulateOduDropdown(),
                PopulateOdutelepDropdown(),
                PopulateLatogatasList()
            );
        }

        protected async Task RefreshAsync()
        {
            IsRefreshing = true;
            await Task.WhenAll(
                PopulateOduDropdown(forceRefresh: true),
                PopulateOdutelepDropdown(forceRefresh: true),
                PopulateLatogatasList(forceRefresh: true)
            ).ContinueWith(t => IsRefreshing = false);
        }

        private async Task PopulateLatogatasList(bool forceRefresh = false)
        {
            if (_settingsService.SelectedOduId == 0)
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

        private async Task PopulateOdutelepDropdown(bool forceRefresh = false)
        {
            OdutelepList = [Odutelep.Empty, .. await _odutelepApi.GetAllOdutelepAsync(forceRefresh)];
            SelectedOdutelep = OdutelepList.FirstOrDefault(x => x.Id == _settingsService.SelectedOdutelepId) ?? OdutelepList[0];
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

        private async Task EditLatogatasAsync(int latogatasId)
        {
            await _navigationService.GoToAsync($"{Constants.RouteEditLatogatas}?{Constants.KeySelectedLatogatasId}={latogatasId}");
        }

        private async Task DeleteLatogatasAsync(int latogatasId)
        {
            if (!await _navigationService.ShowQuestionAsync(AppRes.DeleteLatogatas, AppRes.ConfirmDeleteLatogatas, AppRes.Yes, AppRes.No))
            {
                return;
            }

            var deleted = await _latogatasApi.DeleteLatogatasAsync(latogatasId);

            if (deleted)
            {
                IsBusy = true;
                await PopulateLatogatasList(forceRefresh: true)
                    .ContinueWith(t => IsBusy = false);
            }
            else
            {
                await _navigationService.ShowAlertAsync(AppRes.DeleteFailed);
            }
        }
    }
}

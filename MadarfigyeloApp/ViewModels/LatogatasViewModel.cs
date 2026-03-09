using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Resources;

namespace MadarfigyeloApp.ViewModels
{
    public class LatogatasViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IApiService _apiService;

        private List<Latogatas> _latogatasList = [];
        private List<Odu> _oduList = [Odu.Empty];
        private Odu _selectedOdu;
        private int _selectedOduId = -1;
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

        public LatogatasViewModel(IApiService apiService, INavigationService navigationService) : base(navigationService)
        {            
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            NewLatogatasCommand = new(NewLatogatasAsync);
            RefreshCommand = new(RefreshAsync);
        }

        public override async Task InitAsync()
        {
            await LoadAsync(forceRefresh: false);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey(Constants.ParamOduId) && 
                int.TryParse((string)query[Constants.ParamOduId], out int oduId))
            {
                _selectedOduId = oduId;
            }
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
            SelectedOdu = OduList.FirstOrDefault(x => x.Id == _selectedOduId) ?? OduList[0];

            var latogatasok = await _apiService.GetAllLatogatasAsync(forceRefresh);
            foreach (var latogatas in latogatasok)
            {
                latogatas.Odu = _oduList.FirstOrDefault(x => x.Id == latogatas.OduId);
            }
            LatogatasList = latogatasok;
        }

        private async Task NewLatogatasAsync()
        {
            if (SelectedOdu != Odu.Empty) 
            {
                await _navigationService.GoToAsync($"{Constants.RouteNewLatogatas}?{Constants.ParamOduId}={SelectedOdu.Id}");
            }
            else
            {
                await _navigationService.GoToAsync(Constants.RouteNewLatogatas);
            }
        }
    }
}

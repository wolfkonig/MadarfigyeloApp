using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.ViewModels
{
    public class OdutelepViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        private List<Odutelep> _odutelepList = [];

        public AsyncRelayCommand NewOdutelepCommand { get; private set; }
        public AsyncRelayCommand<int> OdukCommand { get; private set; }
        public AsyncRelayCommand<int> OduMapCommand { get; private set; }

        public List<Odutelep> OdutelepList
        {
            get => _odutelepList;
            set => SetProperty(ref _odutelepList, value);
        }

        public OdutelepViewModel(IApiService apiService, INavigationService navigationService) : base(navigationService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            NewOdutelepCommand = new(NewOdutelep);
            OdukCommand = new(OdukAsync);
            OduMapCommand = new(OduMapAsync);
        }

        public override async Task InitAsync()
        {
            OdutelepList = await _apiService.GetAllOdutelepAsync();
        }

        private async Task NewOdutelep()
        {
            await _navigationService.GoToAsync(Constants.RouteNewOdutelep);
        }

        private async Task OdukAsync(int odutelepId)
        {
            await _navigationService.GoToAsync($"//{Constants.RouteOduk}?{Constants.ParamOduTelepId}={odutelepId}");
        }

        private async Task OduMapAsync(int odutelepId)
        {
            await _navigationService.GoToAsync($"//{Constants.RouteOduMap}?{Constants.ParamOduTelepId}={odutelepId}");
        }
    }
}

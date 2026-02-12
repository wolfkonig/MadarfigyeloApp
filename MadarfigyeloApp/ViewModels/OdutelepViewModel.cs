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

        public List<Odutelep> OdutelepList
        {
            get => _odutelepList;
            set => SetProperty(ref _odutelepList, value);
        }

        public OdutelepViewModel(IApiService apiService, INavigationService navigationService) : base(navigationService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            NewOdutelepCommand = new(NewOdutelep);
        }

        public override async Task InitAsync()
        {
            OdutelepList = await _apiService.GetAllOdutelepAsync();
        }

        private async Task NewOdutelep()
        {
            await _navigationService.GoToAsync(Constants.RouteNewOdutelep);
        }
    }
}

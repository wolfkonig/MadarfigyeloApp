using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.API;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Services;

namespace MadarfigyeloApp.ViewModels
{
    public class OdutelepViewModel : BaseViewModel
    {
        private readonly IOdutelepApi _odutelepApi;

        private List<Odutelep> _odutelepList = [];

        public AsyncRelayCommand NewOdutelepCommand { get; private set; }

        public List<Odutelep> OdutelepList
        {
            get => _odutelepList;
            set => SetProperty(ref _odutelepList, value);
        }

        public OdutelepViewModel(IOdutelepApi odutelepApi, INavigationService navigation, INavigationService navigationService) : base(navigationService)
        {
            _odutelepApi = odutelepApi ?? throw new ArgumentNullException(nameof(odutelepApi));
            _navigationService = navigation ?? throw new ArgumentNullException(nameof(navigation));

            NewOdutelepCommand = new(NewOdutelep);
        }

        public override async Task InitAsync()
        {
            OdutelepList = await _odutelepApi.GetAllOdutelepAsync();
        }

        private async Task NewOdutelep()
        {
            await _navigationService.GoToAsync(Constants.RouteNewOdutelep);
        }
    }
}

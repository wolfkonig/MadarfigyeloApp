using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.ViewModels
{
    public class OduViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        private List<Odu> _oduList = [];
        private List<Odutelep> _odutelepList = [];

        public List<Odu> OduList
        {
            get => _oduList;
            set => SetProperty(ref _oduList, value);
        }

        public AsyncRelayCommand NewOduCommand { get; private set; }

        public OduViewModel(IApiService apiService, INavigationService navigationService) : base(navigationService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            NewOduCommand = new (NewOdu);
        }

        public override async Task InitAsync()
        {
            _odutelepList = await _apiService.GetAllOdutelepAsync();
            var oduk = await _apiService.GetAllOduAsync();
            foreach (var odu in oduk)
            {
                odu.Odutelep = _odutelepList.FirstOrDefault(x => x.Id == odu.OdutelepId);
            }

            OduList = oduk;
        }

        private async Task NewOdu()
        {
            await _navigationService.GoToAsync(Constants.RouteNewOdu);
        }
    }
}

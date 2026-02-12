using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.ViewModels
{
    public class LatogatasViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        private List<Latogatas> _latogatasList = [];
        private List<Odu> _oduList = [];

        public AsyncRelayCommand NewLatogatasCommand { get; private set; }

        public List<Latogatas> LatogatasList
        {
            get => _latogatasList;
            set => SetProperty(ref _latogatasList, value);
        }

        public LatogatasViewModel(IApiService apiService, INavigationService navigationService) : base(navigationService)
        {            
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            NewLatogatasCommand = new(NewLatogatas);
        }

        public override async Task InitAsync()
        {
            _oduList = await _apiService.GetAllOduAsync();
            var latogatasok = await _apiService.GetAllLatogatasAsync();
            foreach (var latogatas in latogatasok)
            {
                latogatas.Odu = _oduList.FirstOrDefault(x => x.Id == latogatas.OduId);
            }
            LatogatasList = latogatasok;
        }

        private async Task NewLatogatas()
        {
            await _navigationService.GoToAsync(Constants.RouteNewLatogatas);
        }
    }
}

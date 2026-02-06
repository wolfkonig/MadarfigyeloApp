using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.API;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Services;

namespace MadarfigyeloApp.ViewModels
{
    public class LatogatasViewModel : BaseViewModel
    {
        private readonly ILatogatasApi _latogatasApi;
        private readonly IOduApi _oduApi;

        private List<Latogatas> _latogatasList = [];
        private List<Odu> _oduList = [];

        public AsyncRelayCommand NewLatogatasCommand { get; private set; }

        public List<Latogatas> LatogatasList
        {
            get => _latogatasList;
            set => SetProperty(ref _latogatasList, value);
        }

        public LatogatasViewModel(ILatogatasApi latogatasApi, IOduApi oduApi, INavigationService navigationService) : base(navigationService)
        {
            _latogatasApi = latogatasApi ?? throw new ArgumentNullException(nameof(latogatasApi));
            _oduApi = oduApi ?? throw new ArgumentNullException(nameof(oduApi));
            NewLatogatasCommand = new(NewLatogatas);
        }

        public override async Task InitAsync()
        {
            _oduList = await _oduApi.GetAllOduAsync();
            var latogatasok = await _latogatasApi.GetAllLatogatasAsync();
            foreach(var latogatas in latogatasok)
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

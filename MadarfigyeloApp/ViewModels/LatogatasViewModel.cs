using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.API;
using MadarfigyeloApp.Models;

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

        public LatogatasViewModel(ILatogatasApi latogatasApi, IOduApi oduApi)
        {
            _latogatasApi = latogatasApi ?? throw new ArgumentNullException(nameof(latogatasApi));
            _oduApi = oduApi ?? throw new ArgumentNullException(nameof(oduApi));
            NewLatogatasCommand = new(NewLatogatas);
        }

        public override async Task InitAsync()
        {
            IsBusy = true;
            _oduList = await _oduApi.GetAllOduAsync();
            var latogatasok = await _latogatasApi.GetAllLatogatasAsync();
            foreach(var latogatas in latogatasok)
            {
                latogatas.Odu = _oduList.FirstOrDefault(x => x.Id == latogatas.OduId);
            }
            LatogatasList = latogatasok;
            IsBusy = false;
        }

        private async Task NewLatogatas()
        {
            IsBusy = true;
            // TODO
            await Task.Delay(5000);
            IsBusy = false;
        }
    }
}

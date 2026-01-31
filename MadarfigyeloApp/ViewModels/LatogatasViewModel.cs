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

        public List<Latogatas> LatogatasList
        {
            get => _latogatasList;
            set => SetProperty(ref _latogatasList, value);
        }

        public LatogatasViewModel(ILatogatasApi latogatasApi, IOduApi oduApi)
        {
            _latogatasApi = latogatasApi ?? throw new ArgumentNullException(nameof(latogatasApi));
            _oduApi = oduApi ?? throw new ArgumentNullException(nameof(oduApi));
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
    }
}

using MadarfigyeloApp.API;
using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.ViewModels
{
    public class OduViewModel : BaseViewModel
    {
        private readonly IOduApi _oduApi;
        private readonly IOdutelepApi _odutelepApi;

        private List<Odu> _oduList = [];
        private List<Odutelep> _odutelepList = [];


        public List<Odu> OduList
        {
            get => _oduList;
            set => SetProperty(ref _oduList, value);
        }

        public OduViewModel(IOduApi oduApi, IOdutelepApi odutelepApi)
        {
            _oduApi = oduApi ?? throw new ArgumentNullException(nameof(oduApi));
            _odutelepApi = odutelepApi ?? throw new ArgumentException(nameof(odutelepApi));
        }

        public override async Task InitAsync()
        {
            _odutelepList = await _odutelepApi.GetAllOdutelepAsync();

            var oduk = await _oduApi.GetAllOduAsync();
            foreach (var odu in oduk)
            {
                odu.Odutelep = _odutelepList.FirstOrDefault(x => x.Id == odu.OdutelepId);
            }

            OduList = oduk;
        }
    }
}

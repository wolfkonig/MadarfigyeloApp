using MadarfigyeloApp.API;
using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.ViewModels
{
    public class OdutelepViewModel : BaseViewModel
    {
        private readonly IOdutelepApi _odutelepApi;
        private List<Odutelep> _odutelepList = [];

        public List<Odutelep> OdutelepList
        {
            get => _odutelepList;
            set => SetProperty(ref _odutelepList, value);
        }

        public OdutelepViewModel(IOdutelepApi odutelepApi)
        {
            _odutelepApi = odutelepApi ?? throw new ArgumentNullException(nameof(odutelepApi));
        }

        public override async Task InitAsync()
        {
            OdutelepList = await _odutelepApi.GetAllOdutelepAsync();
        }
    }
}

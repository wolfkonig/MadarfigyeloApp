using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.API;
using MadarfigyeloApp.Models;

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

        public OdutelepViewModel(IOdutelepApi odutelepApi, IPopupService popupService)
        {
            _odutelepApi = odutelepApi ?? throw new ArgumentNullException(nameof(odutelepApi));
            NewOdutelepCommand = new(NewOdutelep);
        }

        public override async Task InitAsync()
        {
            IsBusy = true;
            OdutelepList = await _odutelepApi.GetAllOdutelepAsync();
            IsBusy = false; 
        }

        private async Task NewOdutelep()
        {
            IsBusy = true;
            // TODO
            await Task.Delay(5000);
            IsBusy = false;
        }
    }
}

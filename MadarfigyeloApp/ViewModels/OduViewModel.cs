using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Resources;

namespace MadarfigyeloApp.ViewModels
{
    public class OduViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        private List<Odu> _oduList = [];
        private List<Odutelep> _odutelepList = [];
        private Odutelep? _selectedOdutelep;

        public List<Odu> OduList
        {
            get => [.. _oduList.Where(o => _selectedOdutelep is null || _selectedOdutelep.Id == 0 || o.OdutelepId == _selectedOdutelep.Id)];
            set => SetProperty(ref _oduList, value);
        }

        public List<Odutelep> OdutelepList
        {
            get => [.. _odutelepList.Concat([new Odutelep { Id = 0, Azonosito = AppRes.NoneSelected }]).OrderBy(x => x.Id)];
            set => SetProperty(ref _odutelepList, value);
        }

        public Odutelep? SelectedOdutelep
        {
            get => _selectedOdutelep;
            set
            {
                SetProperty(ref _selectedOdutelep, value);
                OnPropertyChanged(nameof(OduList));
            }
        }

        public AsyncRelayCommand NewOduCommand { get; private set; }

        public OduViewModel(IApiService apiService, INavigationService navigationService) : base(navigationService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            NewOduCommand = new (NewOdu);
        }

        public override async Task InitAsync()
        {
            OdutelepList = await _apiService.GetAllOdutelepAsync();
            var oduk = await _apiService.GetAllOduAsync();
            foreach (var odu in oduk)
            {
                odu.Odutelep = _odutelepList.FirstOrDefault(x => x.Id == odu.OdutelepId);
            }
            OduList = oduk;
            SelectedOdutelep = OdutelepList.FirstOrDefault(x=>x.Id == 0);
        }

        private async Task NewOdu()
        {
            await _navigationService.GoToAsync(Constants.RouteNewOdu);
        }
    }
}

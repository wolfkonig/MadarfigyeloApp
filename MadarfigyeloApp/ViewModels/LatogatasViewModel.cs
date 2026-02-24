using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Resources;

namespace MadarfigyeloApp.ViewModels
{
    public class LatogatasViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        private List<Latogatas> _latogatasList = [];
        private List<Odu> _oduList = [];
        private Odu? _selectedOdu;

        public AsyncRelayCommand NewLatogatasCommand { get; private set; }

        public List<Latogatas> LatogatasList
        {
            get => [.. _latogatasList.Where(l => _selectedOdu is null || _selectedOdu.Id == 0 || l.OduId == _selectedOdu.Id)];
            set => SetProperty(ref _latogatasList, value);
        }

        public List<Odu> OduList
        {
            get => [.. _oduList.Concat([new Odu { Id = 0, OduAzonosito = AppRes.NoneSelected }]).OrderBy(x => x.Id)];
            set => SetProperty(ref _oduList, value);
        }

        public Odu? SelectedOdu
        {
            get => _selectedOdu;
            set
            {
                SetProperty(ref _selectedOdu, value);
                OnPropertyChanged(nameof(LatogatasList));
            }
        }

        public LatogatasViewModel(IApiService apiService, INavigationService navigationService) : base(navigationService)
        {            
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            NewLatogatasCommand = new(NewLatogatas);
        }

        public override async Task InitAsync()
        {
            OduList = await _apiService.GetAllOduAsync();
            var latogatasok = await _apiService.GetAllLatogatasAsync();
            foreach (var latogatas in latogatasok)
            {
                latogatas.Odu = _oduList.FirstOrDefault(x => x.Id == latogatas.OduId);
            }
            LatogatasList = latogatasok;
            SelectedOdu = OduList.FirstOrDefault(x => x.Id == 0);
        }

        private async Task NewLatogatas()
        {
            await _navigationService.GoToAsync(Constants.RouteNewLatogatas);
        }
    }
}

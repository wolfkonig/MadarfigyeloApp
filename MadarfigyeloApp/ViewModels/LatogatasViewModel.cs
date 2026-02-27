using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Resources;

namespace MadarfigyeloApp.ViewModels
{
    public class LatogatasViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IApiService _apiService;

        private List<Latogatas> _latogatasList = [];
        private List<Odu> _oduList = [Odu.Empty];
        private Odu _selectedOdu;
        private int _selectedOduId = -1;

        public AsyncRelayCommand NewLatogatasCommand { get; private set; }

        public List<Latogatas> LatogatasList
        {
            get => [.. _latogatasList.Where(l => _selectedOdu is null || _selectedOdu.Id == 0 || l.OduId == _selectedOdu.Id)];
            set => SetProperty(ref _latogatasList, value);
        }

        public List<Odu> OduList
        {
            get => _oduList;
            set => SetProperty(ref _oduList, value);
        }

        public Odu SelectedOdu
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
            SelectedOdu = OduList[0];
        }

        public override async Task InitAsync()
        {
            var oduk = await _apiService.GetAllOduAsync();
            OduList = [.. oduk.Concat([Odu.Empty]).OrderBy(x => x.Id)];
            SelectedOdu = OduList.FirstOrDefault(x => x.Id == _selectedOduId) ?? OduList[0];

            var latogatasok = await _apiService.GetAllLatogatasAsync();
            foreach (var latogatas in latogatasok)
            {
                latogatas.Odu = _oduList.FirstOrDefault(x => x.Id == latogatas.OduId);
            }
            LatogatasList = latogatasok;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey(Constants.ParamOduId) && 
                int.TryParse((string)query[Constants.ParamOduId], out int oduId))
            {
                _selectedOduId = oduId;
            }
        }

        private async Task NewLatogatas()
        {
            if (SelectedOdu != Odu.Empty) 
            {
                await _navigationService.GoToAsync($"{Constants.RouteNewLatogatas}?{Constants.ParamOduId}={SelectedOdu.Id}");
            }
            else
            {
                await _navigationService.GoToAsync(Constants.RouteNewLatogatas);
            }
        }
    }
}

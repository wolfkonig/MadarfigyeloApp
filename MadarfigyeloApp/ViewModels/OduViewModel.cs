using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Resources;

namespace MadarfigyeloApp.ViewModels
{
    public class OduViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IApiService _apiService;

        private List<Odu> _oduList = [];
        private List<Odutelep> _odutelepList = [Odutelep.Empty];
        private Odutelep _selectedOdutelep;
        private int _selectedOdutelepId = -1;

        public List<Odu> OduList
        {
            get => [.. _oduList.Where(o => SelectedOdutelep == null || SelectedOdutelep.Id == 0 || o.OdutelepId == SelectedOdutelep.Id)];
            set => SetProperty(ref _oduList, value);
        }

        public List<Odutelep> OdutelepList
        {
            get => _odutelepList;
            set => SetProperty(ref _odutelepList, value);
        }

        public Odutelep SelectedOdutelep
        {
            get => _selectedOdutelep;
            set
            {
                SetProperty(ref _selectedOdutelep, value);
                OnPropertyChanged(nameof(OduList));
            }
        }

        public AsyncRelayCommand NewOduCommand { get; private set; }
        public AsyncRelayCommand<int> LatogatasokCommand { get; private set; }

        public OduViewModel(IApiService apiService, INavigationService navigationService) : base(navigationService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            NewOduCommand = new(NewOduAsync);
            LatogatasokCommand = new(LatogatasokAsync);
        }

        public override async Task InitAsync()
        {
            var odutelepek = await _apiService.GetAllOdutelepAsync();
            OdutelepList = [.. odutelepek.Concat([Odutelep.Empty]).OrderBy(x => x.Id)];
            SelectedOdutelep = OdutelepList.SingleOrDefault(x => x.Id == _selectedOdutelepId) ?? OdutelepList[0];

            var oduk = await _apiService.GetAllOduAsync();
            foreach (var odu in oduk)
            {
                odu.Odutelep = _odutelepList.FirstOrDefault(x => x.Id == odu.OdutelepId);
            }
            OduList = oduk;
        }

        private async Task NewOduAsync()
        {
            if (SelectedOdutelep != Odutelep.Empty)
            {
                await _navigationService.GoToAsync($"{Constants.RouteNewOdu}?{Constants.ParamOduTelepId}={SelectedOdutelep.Id}");
            }
            else
            {
                await _navigationService.GoToAsync(Constants.RouteNewOdu);
            }
        }

        private async Task LatogatasokAsync(int oduId)
        {
            await _navigationService.GoToAsync($"//{Constants.RouteLatogatasok}?{Constants.ParamOduId}={oduId}");
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey(Constants.ParamOduTelepId) &&
                int.TryParse((string)query[Constants.ParamOduTelepId], out int odutelepId))
            {
                _selectedOdutelepId = odutelepId;
            }
        }
    }
}

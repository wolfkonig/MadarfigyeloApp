using Terepnaplo.Models;
using Terepnaplo.Resources;
using Terepnaplo;
using Terepnaplo.Contracts;

namespace Terepnaplo.ViewModels
{
    public class EditLatogatasViewModel(
        IOduApiService oduApiService,
        ILatogatasApiService latogatasApiService,
        INavigationService navigationService,
        ISettingsService settingsService) : NewLatogatasViewModel(oduApiService, latogatasApiService, navigationService, settingsService), IQueryAttributable
    {
        private int _selectedLatogatasId = 0;
        public override string PageTitle => AppRes.EditLatogatas;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(Constants.KeySelectedLatogatasId, out object? value) 
                && int.TryParse(value?.ToString(), out int id))
            {
                _selectedLatogatasId = id;
            }
        }

        public override async Task InitAsync()
        {
            await base.InitAsync();

            if (_selectedLatogatasId == 0)
            {
                return;
            }

            var latogatas = await _latogatasApi.GetLatogatasAsync(_selectedLatogatasId);
            if (latogatas is null)
            {
                return;
            }

            SelectedOdu = Oduk.FirstOrDefault(x => x.Id == latogatas.OduId);
            Datum = latogatas.Datum;
            Tevekenyseg = latogatas.Tevekenyseg;
            Allapot = latogatas.Allapot;
            Faj = latogatas.Faj;
            TojasSzam = latogatas.TojasSzam;
            FiokaSzam = latogatas.FiokaSzam;
            FiokakKora = latogatas.FiokakKora;
            Megjegyzesek = latogatas.Megjegyzesek;
        }

        protected override async Task SaveAsync()
        {
            if (!await Validate())
            {
                return;
            }

            var latogatas = new Latogatas
            {
                Id = _selectedLatogatasId,
                OduId = SelectedOdu.Id,
                Datum = Datum,
                Tevekenyseg = Tevekenyseg.Value,
                Allapot = Allapot.Value,
                Faj = Faj ?? string.Empty,
                TojasSzam = TojasSzam,
                FiokaSzam = FiokaSzam,
                FiokakKora = FiokakKora,
                Megjegyzesek = Megjegyzesek
            };

            var success = await _latogatasApi.UpdateLatogatasAsync(latogatas);

            if (success)
            {
                await _navigationService
                    .ShowAlertAsync(string.Format(AppRes.SaveSuccessful, AppRes.Latogatas))
                    .ContinueWith(_ => _navigationService.PopAsync());
            }
            else
            {
                await _navigationService.ShowAlertAsync(AppRes.UpdateFailed);
            }
        }
    }
}
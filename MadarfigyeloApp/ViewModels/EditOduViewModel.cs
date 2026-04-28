using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Resources;

namespace MadarfigyeloApp.ViewModels
{
    public class EditOduViewModel(
        IOduApiService oduApiService,
        IOdutelepApiService odutelepApiService,
        INavigationService navigationService,
        ILocationService locationService,
        ISettingsService settingsService,
        ILoggerService logger) : NewOduViewModel(oduApiService, odutelepApiService, navigationService, locationService, settingsService, logger)
    {
        public override async Task InitAsync()
        {
            if (_settingsService.SelectedOduId != 0)
            {
                var odu = await _oduApi.GetOduAsync(_settingsService.SelectedOduId);
                if (odu is not null)
                {
                    OduAzonosito = odu.OduAzonosito;
                    OduTipus = odu.OduTipus;
                    BejaratiNyilasMm = odu.BejaratiNyilasMm;
                    Elohelykod = odu.Elohelykod;
                    MireVanHelyezve = odu.MireVanHelyezve;
                    FelhelyezesModja = odu.FelhelyezesModja;
                    OduTajolasa = odu.OduTajolasa;
                    OdutTartoNovenyfaj = odu.OdutTartoNovenyfaj;
                    MagassagMeter = odu.MagassagMeter;

                    if (_settingsService.SelectedOdutelepId == 0)
                    {
                        _settingsService.SelectedOdutelepId = odu.OdutelepId;
                    }
                }
            }
            await base.InitAsync();
        }

        public override string PageTitle => AppRes.EditOdu;

        protected override async Task SaveAsync()
        {
            if (!await Validate())
            {
                return;
            }

            var odu = new Odu
            {
                Id = _settingsService.SelectedOduId,
                OduAzonosito = OduAzonosito,
                OdutelepId = SelectedOdutelep.Id,
                OduTipus = OduTipus,
                BejaratiNyilasMm = BejaratiNyilasMm,
                GpsLatitude = GpsLatitude,
                GpsLongitude = GpsLongitude,
                Elohelykod = Elohelykod,
                MireVanHelyezve = MireVanHelyezve,
                FelhelyezesModja = FelhelyezesModja,
                OduTajolasa = OduTajolasa,
                OdutTartoNovenyfaj = OdutTartoNovenyfaj,
                MagassagMeter = MagassagMeter
            };

            var success = await _oduApi.UpdateOduAsync(odu);

            if (success)
            {
                await _navigationService.ShowAlertAsync(string.Format(AppRes.SaveSuccessful, AppRes.Odu));
                await _navigationService.PopAsync();
            }
        }
    }
}

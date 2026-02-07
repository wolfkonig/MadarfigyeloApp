using MadarfigyeloApp.Views;

namespace MadarfigyeloApp.Services
{
    public interface INavigationService
    {
        public Task GoToAsync(string route);

        public Task PushAsync(BasePage page);

        public Task PopAsync();

        public Task ShowAlert(string message);
    }

    public class ShellNavigationService : INavigationService
    {
        public async Task GoToAsync(string route)
        {
            await Shell.Current.GoToAsync(route);
        }

        public async Task PopAsync()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        public async Task PushAsync(BasePage page)
        {
            await Shell.Current.Navigation.PushAsync(page);
        }

        public async Task ShowAlert(string message)
        {
            await Shell.Current.DisplayAlert(null, message, Resources.AppRes.OK);
        }
    }
}

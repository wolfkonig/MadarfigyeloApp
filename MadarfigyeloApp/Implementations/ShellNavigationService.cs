using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Views;

namespace MadarfigyeloApp.Implementations
{
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

        public async Task ShowAlertAsync(string message, string? title = null)
        {
            await Shell.Current.DisplayAlert(title, message, Resources.AppRes.OK);           
        }

        public async Task<bool> ShowQuestionAsync(string title, string message)
        {
            return await Shell.Current.DisplayAlert(title, message, Resources.AppRes.OK, Resources.AppRes.Cancel);
        }
    }
}

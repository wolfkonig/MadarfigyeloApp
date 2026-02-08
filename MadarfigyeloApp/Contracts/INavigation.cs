using MadarfigyeloApp.Views;

namespace MadarfigyeloApp.Contracts
{
    public interface INavigationService
    {
        public Task GoToAsync(string route);

        public Task PushAsync(BasePage page);

        public Task PopAsync();

        public Task ShowAlert(string message);
    }
}

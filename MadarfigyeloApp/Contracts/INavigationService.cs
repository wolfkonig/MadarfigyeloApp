using MadarfigyeloApp.Views;

namespace MadarfigyeloApp.Contracts
{
    public interface INavigationService
    {
        public Task GoToAsync(string route);

        public Task PushAsync(BasePage page);

        public Task PopAsync();

        public Task ShowAlertAsync(string message, string? title = null);

        public Task<bool> ShowQuestionAsync(string title, string message);

        public Task<bool> ShowQuestionAsync(string title, string message, string acceptButtonText, string cancelButtonText);
    }
}

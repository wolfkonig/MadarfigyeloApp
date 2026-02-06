using MadarfigyeloApp.Services;

namespace MadarfigyeloApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override Task InitAsync()
        {
            return Task.CompletedTask;
        }
    }
}

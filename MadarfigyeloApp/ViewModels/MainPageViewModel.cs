using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Resources;

namespace MadarfigyeloApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        public AsyncRelayCommand LogoutCommand { get; }

        public MainPageViewModel(INavigationService navigationService, IUserService userService) : base(navigationService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            LogoutCommand = new(async () =>
            {
               var confirmed = await _navigationService.ShowQuestionAsync(AppRes.Logout, AppRes.LogoutConfirm);
                if (confirmed)
                {
                    await _userService.LogOutUser();
                    await _navigationService.GoToAsync($"//{Constants.RouteLogin}");
                }
            });
        }

        public override Task InitAsync()
        {
            return Task.CompletedTask;
        }
    }
}

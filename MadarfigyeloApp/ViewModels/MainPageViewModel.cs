using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Resources;

namespace MadarfigyeloApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private string loggedInUser;

        public AsyncRelayCommand LogoutCommand { get; }

        public string LoggedInUser
        {
            get => loggedInUser;
            set => SetProperty(ref loggedInUser, value);
        }

        public MainPageViewModel(INavigationService navigationService, IUserService userService) : base(navigationService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            LogoutCommand = new(async () =>
            {
               var confirmed = await _navigationService.ShowQuestionAsync(AppRes.Logout, AppRes.LogoutConfirm);
                if (confirmed)
                {
                    _userService.LogOutUser();
                    await _navigationService.GoToAsync($"//{Constants.RouteLogin}");
                }
            });

            LoggedInUser = _userService.GetLoggedInUser()?.Email ?? string.Empty;
        }

        public override Task InitAsync()
        {
            return Task.CompletedTask;
        }
    }
}

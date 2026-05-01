using CommunityToolkit.Mvvm.Input;
using Terepnaplo.Resources;
using Terepnaplo;
using Terepnaplo.Contracts;

namespace Terepnaplo.ViewModels
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

        public string Version { get; }

        public MainPageViewModel(INavigationService navigationService, IUserService userService) : base(navigationService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            LogoutCommand = new(() => LogoutAsync(true));
            Version = AppInfo.VersionString;
        }

        public override async Task InitAsync()
        {
            var loggedinUser = _userService.GetLoggedInUser();
            if (loggedinUser == null)
            {
                await LogoutAsync(false);
            }
            else
            {
                LoggedInUser = loggedinUser.Email ?? string.Empty;
                return;
            }
        }

        private async Task LogoutAsync(bool needConfirmation)
        {
            var confirmed = !needConfirmation || await _navigationService.ShowQuestionAsync(AppRes.Logout, AppRes.LogoutConfirm);
            if (confirmed)
            {
                _userService.LogOutUser();
                await _navigationService.GoToAsync($"//{Constants.RouteLogin}");
            }
        }
    }
}

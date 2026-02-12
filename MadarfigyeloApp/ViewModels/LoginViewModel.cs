using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Resources;

namespace MadarfigyeloApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private static IUserService _userService;
        private string _loginName;
        private string _password;

        public AsyncRelayCommand LoginCommand { get; set; }

        public LoginViewModel(INavigationService navigationService, IUserService userService) : base(navigationService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            LoginCommand = new AsyncRelayCommand(LoginAsync);
        }

        public string LoginName
        {
            get => _loginName;
            set => SetProperty(ref _loginName, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public override Task InitAsync()
        {
            return Task.CompletedTask;
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrEmpty(_loginName))
            {
                await _navigationService.ShowAlertAsync(string.Format(AppRes.ErrorEmpty, AppRes.LoginName));
                return;
            }
            if (string.IsNullOrEmpty(_password))
            {
                await _navigationService.ShowAlertAsync(string.Format(AppRes.ErrorEmpty, AppRes.Password));
                return;
            }

            if (await _userService.LogInUser(LoginName, Password))
            {
                await _navigationService.GoToAsync($"//{Constants.RouteHome}");
            }
            else
            {
                await _navigationService.ShowAlertAsync(AppRes.WrongLoginOrPassword, AppRes.LoginFailed);
            }
        }
    }
}

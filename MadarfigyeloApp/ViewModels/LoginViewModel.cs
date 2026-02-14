using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Resources;

namespace MadarfigyeloApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private static IUserService _userService;
        private string _loginEmail;
        private string _password;
        private string _passwordAgain;
        private bool _isRegistering;

        public AsyncRelayCommand LoginCommand { get; }
        public AsyncRelayCommand RegisterCommand { get; }

        public LoginViewModel(INavigationService navigationService, IUserService userService) : base(navigationService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            LoginCommand = new AsyncRelayCommand(LoginAsync);
        }

        public bool IsRegistering 
        { 
            get => _isRegistering; 
            set => SetProperty(ref _isRegistering, value);
        }

        public string LoginEmail
        {
            get => _loginEmail;
            set => SetProperty(ref _loginEmail, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string PasswordAgain
        {
            get => _passwordAgain;
            set => SetProperty(ref _passwordAgain, value);
        }

        public override Task InitAsync()
        {
            return Task.CompletedTask;
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrEmpty(_loginEmail))
            {
                await _navigationService.ShowAlertAsync(string.Format(AppRes.ErrorEmpty, AppRes.LoginName));
                return;
            }
            if (string.IsNullOrEmpty(_password))
            {
                await _navigationService.ShowAlertAsync(string.Format(AppRes.ErrorEmpty, AppRes.Password));
                return;
            }

            if (await _userService.LogInUser(LoginEmail, Password))
            {
                await _navigationService.GoToAsync($"//{Constants.RouteHome}");
            }
            else
            {
                await _navigationService.ShowAlertAsync(AppRes.WrongLoginOrPassword, AppRes.LoginFailed);
            }
        }

        private async Task RegisterUser()
        {

        }
    }
}

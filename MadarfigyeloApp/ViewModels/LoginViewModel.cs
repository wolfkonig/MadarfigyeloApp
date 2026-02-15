using CommunityToolkit.Mvvm.Input;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Resources;

namespace MadarfigyeloApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private string _loginEmail = string.Empty;
        private string _password = string.Empty;
        private string _passwordAgain = string.Empty;
        private bool _isRegistering;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;

        public AsyncRelayCommand LoginCommand { get; }
        public AsyncRelayCommand RegisterCommand { get; }
        public RelayCommand ToggleRegisterCommand { get; }

        public LoginViewModel(INavigationService navigationService, IUserService userService) : base(navigationService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            LoginCommand = new AsyncRelayCommand(LoginAsync);
            RegisterCommand = new AsyncRelayCommand(RegisterUser);
            ToggleRegisterCommand = new RelayCommand(() =>
            {
                IsRegistering = !IsRegistering;
                _errors.Clear();
            });
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

        public string FirstName 
        { 
            get => _firstName; 
            set => SetProperty(ref _firstName, value);
        }

        public string LastName 
        { 
            get => _lastName; 
            set => SetProperty(ref _lastName, value);
        }

        public override Task InitAsync()
        {
            return Task.CompletedTask;
        }

        private async Task LoginAsync()
        {

            ShowLoading("Logging in...");
            if (await _userService.LogInUser(LoginEmail, Password))
            {
                await _navigationService.GoToAsync($"//{Constants.RouteHome}");
            }
            else
            {
                await _navigationService.ShowAlertAsync(AppRes.WrongLoginOrPassword, AppRes.LoginFailed);
                HideLoading();
            }
        }

        private async Task RegisterUser()
        {

        }

        private bool ValidateRegistration()
        {
            _errors.Clear();
            
            if (string.IsNullOrWhiteSpace(LoginEmail))
            {
                _errors.Add(string.Format(AppRes.ErrorEmpty, AppRes.Email));
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                _errors.Add(string.Format(AppRes.ErrorEmpty, AppRes.Password));
            }

            if (IsRegistering && string.IsNullOrWhiteSpace(FirstName))
            {
                _errors.Add(string.Format(AppRes.ErrorEmpty, AppRes.FirstName));
            }
            if (IsRegistering && string.IsNullOrWhiteSpace(LastName))
            {
                _errors.Add(string.Format(AppRes.ErrorEmpty, AppRes.LastName));
            }
            if (IsRegistering && !IsValidEmail(LoginEmail))
            {
                _errors.Add(AppRes.InvalidEmailFormat);
            }
            else if (IsRegistering && Password.Length < 6)
            {
                _errors.Add(AppRes.PasswordTooShort);
            }
            if (IsRegistering && Password != PasswordAgain)
            {
                _errors.Add(AppRes.PasswordsDoNotMatch);
            }
            return _errors.Count == 0;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

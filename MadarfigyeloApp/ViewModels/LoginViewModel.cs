using CommunityToolkit.Mvvm.Input;
using Terepnaplo.Models;
using Terepnaplo.Resources;
using System.Text.RegularExpressions;
using Terepnaplo;
using Terepnaplo.Contracts;
using Terepnaplo.ViewModels;

namespace Terepnaplo.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
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
            if(!await Validate())
            {
                return;
            }
            bool success;
            IsBusy = true;
            try
            {
                success = await _userService.LogInUser(LoginEmail, Password);
            }
            finally
            {
                IsBusy = false;
            }

            if (success)
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
            if (await Validate())
            {
                var newUser = new UserDto(LoginEmail)
                {
                    Password = Password,
                    FirstName = FirstName,
                    LastName = LastName
                };

                var success = await _userService.RegisterUser(newUser);
                IsBusy = false;
                IsRegistering = !success;
            }
        }


        private async Task<bool> Validate()
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

            if (IsRegistering)
            {
                if (string.IsNullOrWhiteSpace(FirstName))
                {
                    _errors.Add(string.Format(AppRes.ErrorEmpty, AppRes.FirstName));
                }
                if (string.IsNullOrWhiteSpace(LastName))
                {
                    _errors.Add(string.Format(AppRes.ErrorEmpty, AppRes.LastName));
                }
                if (!IsValidEmail(LoginEmail))
                {
                    _errors.Add(AppRes.InvalidEmailFormat);
                }
                else if (!PasswordRegex().IsMatch(Password))
                {
                    _errors.Add(AppRes.PasswordTooShort);
                }

                if (Password != PasswordAgain)
                {
                    _errors.Add(AppRes.PasswordsDoNotMatch);
                }
            }

            if (HasErrors)
            {
                var message = _errors.Aggregate((a, b) => $"{a}\r\n{b}");
                await _navigationService.ShowAlertAsync(message);
                return false;
            }
            return true;
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

        [GeneratedRegex("^(?=.*[A-Z])(?=.*\\d)[A-Za-z\\d]{6,}$")]
        private static partial Regex PasswordRegex();
    }
}

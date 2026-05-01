using CommunityToolkit.Mvvm.ComponentModel;
using Terepnaplo.Resources;
using Terepnaplo.Contracts;

namespace Terepnaplo.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        private bool _isBusy;

        protected INavigationService _navigationService;
        protected List<string> _errors = [];
        private string _loadingMessage;

        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            LoadingMessage = string.Empty;
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool HasErrors => _errors.Any();

        public string LoadingMessage
        {
            get
            {
                if (string.IsNullOrEmpty(_loadingMessage))
                {
                    return AppRes.Loading;
                }
                return _loadingMessage;
            }

            set => SetProperty(ref _loadingMessage, value);
        }

        public abstract Task InitAsync();
    }
}

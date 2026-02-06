using CommunityToolkit.Mvvm.ComponentModel;
using MadarfigyeloApp.Services;


namespace MadarfigyeloApp.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        private bool _isBusy;

        protected INavigationService _navigationService;

        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public bool IsBusy 
        { 
            get => _isBusy; 
            set => SetProperty(ref _isBusy, value);
        }

        public abstract Task InitAsync();
    }
}

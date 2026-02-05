using CommunityToolkit.Mvvm.ComponentModel;


namespace MadarfigyeloApp.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        private bool _isBusy;

        public bool IsBusy 
        { 
            get => _isBusy; 
            set => SetProperty(ref _isBusy, value);
        }

        public abstract Task InitAsync();
    }
}

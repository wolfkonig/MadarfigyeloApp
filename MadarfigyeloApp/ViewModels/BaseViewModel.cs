using CommunityToolkit.Mvvm.ComponentModel;


namespace MadarfigyeloApp.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        public abstract Task InitAsync();
    }
}

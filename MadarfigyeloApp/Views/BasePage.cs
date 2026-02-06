using MadarfigyeloApp.ViewModels;

namespace MadarfigyeloApp.Views;

public abstract class BasePage : ContentPage
{
	public BasePage(BaseViewModel vm)
	{
		BindingContext = vm ?? throw new ArgumentNullException("Missing ViewModel");
        Appearing += async (_, _) =>
        {
            vm.IsBusy = true;
            await vm.InitAsync();
            vm.IsBusy = false;
        };

        // Adding ControlTemplate to show Activity Indicator
        if (Application.Current is not null && 
            Application.Current.Resources.TryGetValue("MainPageTemplate", out var resource) && 
            resource is ControlTemplate template)
        {
            ControlTemplate = template;
        }
    }
}
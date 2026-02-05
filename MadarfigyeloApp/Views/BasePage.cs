using MadarfigyeloApp.ViewModels;

namespace MadarfigyeloApp.Views;

public abstract class BasePage : ContentPage
{
	public BasePage(BaseViewModel vm)
	{
		BindingContext = vm;
		Appearing += async (_, _) => await vm.InitAsync();

        // Adding ControlTemplate to show Activity Indicator
        if (Application.Current.Resources.TryGetValue("MainPageTemplate", out var resource) && 
            resource is ControlTemplate template)
        {
            ControlTemplate = template;
        }
    }
}
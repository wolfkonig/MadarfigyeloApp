using MadarfigyeloApp.ViewModels;

namespace MadarfigyeloApp.Views;

public abstract class BasePage : ContentPage
{
	public BasePage(BaseViewModel vm)
	{
		BindingContext = vm;
		Appearing += async (_, _) => await vm.InitAsync();
	}
}
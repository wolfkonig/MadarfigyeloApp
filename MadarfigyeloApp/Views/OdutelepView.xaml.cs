using MadarfigyeloApp.ViewModels;

namespace MadarfigyeloApp.Views;

public partial class OdutelepView : ContentPage
{
	public OdutelepView(OdutelepViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();

        Appearing += async (_, _) => await vm.Init();
    }
}
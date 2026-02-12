using MadarfigyeloApp.ViewModels;

namespace MadarfigyeloApp.Views;

public partial class LoginView : BasePage
{
	public LoginView(LoginViewModel vm) : base(vm)
	{
		InitializeComponent();
	}
}
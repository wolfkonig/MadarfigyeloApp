using Terepnaplo.ViewModels;
using Terepnaplo.Views;

namespace Terepnaplo.Views;

public partial class LoginView : BasePage
{
	public LoginView(LoginViewModel vm) : base(vm)
	{
		InitializeComponent();
	}
}
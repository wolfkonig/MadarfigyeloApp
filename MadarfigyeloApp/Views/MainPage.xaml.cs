using Terepnaplo.ViewModels;
using Terepnaplo.Views;

namespace Terepnaplo.Views
{
    public partial class MainPage : BasePage
    {
        public MainPage(MainPageViewModel vm) : base(vm)
        {
            InitializeComponent();
        }
    }
}

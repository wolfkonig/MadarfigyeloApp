using MadarfigyeloApp.ViewModels;
using MadarfigyeloApp.Views;

namespace MadarfigyeloApp
{
    public partial class MainPage : BasePage
    {
        public MainPage(MainPageViewModel vm) : base(vm)
        {
            InitializeComponent();
        }
    }
}

using MadarfigyeloApp.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadarfigyeloApp.ViewModels
{
    public class OduMapViewModel : BaseViewModel
    {
        public OduMapViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override Task InitAsync()
        {
            return Task.CompletedTask;
        }
    }
}

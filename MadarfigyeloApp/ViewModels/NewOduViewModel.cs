using CommunityToolkit.Maui;
using MadarfigyeloApp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadarfigyeloApp.ViewModels
{
    public class NewOduViewModel : BaseViewModel
    {
        private readonly IOduApi _oduApi;

        public NewOduViewModel(IOduApi oduApi)
        {
            _oduApi = oduApi ?? throw new ArgumentNullException(nameof(oduApi));
        }

        public override Task InitAsync()
        {
            return Task.CompletedTask;
        }
    }
}

using MadarfigyeloApp.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadarfigyeloApp.API
{
    public interface IAuthApi
    {
        [Post("/auth/login")]
        Task<AuthResponse> LogInUser(string email, string password);

        [Post("/auth/register")]
        Task<AuthResponse> RegisterUser([Body] User user);
    }
}

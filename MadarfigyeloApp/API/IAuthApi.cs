using MadarfigyeloApp.Models;
using Refit;

namespace MadarfigyeloApp.API
{
    public interface IAuthApi
    {
        [Post("/auth/login")]
        Task<IApiResponse<AuthResponseDto>> LogInUser([Body] LoginDto login);

        [Post("/auth/register")]
        Task<IApiResponse<AuthResponseDto>> RegisterUser([Body] UserDto user);
    }
}

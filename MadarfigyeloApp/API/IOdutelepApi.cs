using MadarfigyeloApp.Models;
using Refit;

namespace MadarfigyeloApp.API;

public interface IOdutelepApi
{
    [Get("/Odutelep/{id}")]
    Task<ApiResponse<Odutelep>> GetOdutelepAsync(int id);

    [Get("/Odutelep")]
    Task<ApiResponse<List<Odutelep>>> GetAllOdutelepAsync();

    [Post("/Odutelep")]
    Task<IApiResponse> PostOdutelepAsync([Body] Odutelep odutelep);
}
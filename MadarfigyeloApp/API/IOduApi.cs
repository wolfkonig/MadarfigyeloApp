using MadarfigyeloApp.Models;
using Refit;

namespace MadarfigyeloApp.API;

public interface IOduApi
{
    [Get("/Odu/{id}")]
    Task<ApiResponse<Odu>> GetOduAsync(int id);

    [Get("/Odu")]
    Task<ApiResponse<List<Odu>>> GetAllOduAsync();

    [Post("/Odu")]
    Task<IApiResponse> PostOduAsync([Body] Odu odu);
}
using MadarfigyeloApp.Models;
using Refit;

namespace MadarfigyeloApp.API
{
    public interface ILatogatasApi
    {
        [Get("/Latogatas/{id}")]
        Task<ApiResponse<Latogatas>> GetLatogatasAsync(int id);

        [Get("/Latogatas")]
        Task<ApiResponse<List<Latogatas>>> GetAllLatogatasAsync();

        [Post("/Latogatas")]
        Task<IApiResponse> PostLatogatasAsync([Body] Latogatas latogatas);
    }
}

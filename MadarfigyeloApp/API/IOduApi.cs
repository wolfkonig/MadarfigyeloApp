using MadarfigyeloApp.Models;
using Refit;

namespace MadarfigyeloApp.API;

public interface IOduApi
{
    [Get("/Odu/{id}")]
    Task<Odu> GetOduAsync(int id);

    [Get("/Odu")]
    Task<List<Odu>> GetAllOduAsync();

    [Post("/Odu")]
    Task PostOduAsync([Body] Odu odu);
}
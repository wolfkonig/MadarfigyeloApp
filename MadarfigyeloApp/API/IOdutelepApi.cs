using MadarfigyeloApp.Models;
using Refit;

namespace MadarfigyeloApp.API;

public interface IOdutelepApi
{
    [Get("/Odutelep/{id}")]
    Task<Odutelep> GetOdutelepAsync(int id);

    [Get("/Odutelep")]
    Task<List<Odutelep>> GetAllOdutelepAsync();

    [Post("/Odutelep")]
    Task PostOdutelepAsync([Body] Odutelep odutelep);
}
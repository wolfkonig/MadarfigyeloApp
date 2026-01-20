using MadarfigyeloApp.Models;
using Refit;

namespace MadarfigyeloApp.API
{
    public interface ILatogatasApi
    {
        [Get("/Latogatas/{id}")]
        Task<Latogatas> GetLatogatasAsync(int id);

        [Get("/Latogatas")]
        Task<List<Latogatas>> GetAllLatogatasAsync();
    }
}

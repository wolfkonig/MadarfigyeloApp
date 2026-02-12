using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.Contracts
{
    public interface IApiService
    {
        Task<Latogatas?> GetLatogatasAsync(int id);
        Task<List<Latogatas>> GetAllLatogatasAsync();
        Task<bool> PostLatogatasAsync(Latogatas latogatas);

        Task<List<Odu>> GetAllOduAsync();
        Task<Odu?> GetOduAsync(int id);
        Task<bool> PostOduAsync(Odu odu);

        Task<Odutelep?> GetOdutelepAsync(int id);
        Task<List<Odutelep>> GetAllOdutelepAsync();
        Task<bool> PostOdutelepAsync(Odutelep odutelep);
    }
}

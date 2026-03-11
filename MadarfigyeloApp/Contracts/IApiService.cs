using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.Contracts
{
    public interface IApiService
    {
        Task<Latogatas?> GetLatogatasAsync(int id);
        Task<List<Latogatas>> GetAllLatogatasAsync(bool forceRefresh = false);
        Task<List<Latogatas>> GetLatogatasByOduAsync(int oduId, bool forceRefresh = false);
        Task<bool> PostLatogatasAsync(Latogatas latogatas);

        Task<List<Odu>> GetAllOduAsync(bool forceRefresh = false);
        Task<List<Odu>> GetOduByOdutelepAsync(int odutelepId, bool forceRefresh = false);
        Task<Odu?> GetOduAsync(int id);
        Task<bool> PostOduAsync(Odu odu);

        Task<Odutelep?> GetOdutelepAsync(int id);
        Task<List<Odutelep>> GetAllOdutelepAsync(bool forceRefresh = false);
        Task<bool> PostOdutelepAsync(Odutelep odutelep);
    }
}

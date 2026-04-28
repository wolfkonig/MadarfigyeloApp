using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.Contracts
{
    public interface IOduApiService
    {
        Task<Odu?> GetOduAsync(int id);
        Task<List<Odu>> GetAllOduAsync(bool forceRefresh = false);
        Task<List<Odu>> GetOduByOdutelepAsync(int odutelepId, bool forceRefresh = false);
        Task<bool> PostOduAsync(Odu odu);
        Task<bool> UpdateOduAsync(Odu odu);
        Task<bool> DeleteOduAsync(int id);
    }
}

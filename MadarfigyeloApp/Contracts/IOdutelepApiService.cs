using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.Contracts
{
    public interface IOdutelepApiService
    {
        Task<Odutelep?> GetOdutelepAsync(int id);
        Task<List<Odutelep>> GetAllOdutelepAsync(bool forceRefresh = false);
        Task<bool> PostOdutelepAsync(Odutelep odutelep);
    }
}

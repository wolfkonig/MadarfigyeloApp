using Terepnaplo.Models;

namespace Terepnaplo.Contracts
{
    public interface IOdutelepApiService
    {
        Task<Odutelep?> GetOdutelepAsync(int id);
        Task<List<Odutelep>> GetAllOdutelepAsync(bool forceRefresh = false);
        Task<bool> PostOdutelepAsync(Odutelep odutelep);
    }
}

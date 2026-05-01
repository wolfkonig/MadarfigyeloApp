using Terepnaplo.Models;

namespace Terepnaplo.Contracts
{
    public interface ILatogatasApiService
    {
        Task<Latogatas?> GetLatogatasAsync(int id);
        Task<List<Latogatas>> GetAllLatogatasAsync(bool forceRefresh = false);
        Task<List<Latogatas>> GetLatogatasByOduAsync(int oduId, bool forceRefresh = false);
        Task<bool> PostLatogatasAsync(Latogatas latogatas);
        Task<bool> UpdateLatogatasAsync(Latogatas latogatas);
        Task<bool> DeleteLatogatasAsync(int id);
    }
}

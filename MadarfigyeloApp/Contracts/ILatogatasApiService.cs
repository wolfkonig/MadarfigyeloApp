using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.Contracts
{
    public interface ILatogatasApiService
    {
        Task<Latogatas?> GetLatogatasAsync(int id);
        Task<List<Latogatas>> GetAllLatogatasAsync(bool forceRefresh = false);
        Task<List<Latogatas>> GetLatogatasByOduAsync(int oduId, bool forceRefresh = false);
        Task<bool> PostLatogatasAsync(Latogatas latogatas);
    }
}

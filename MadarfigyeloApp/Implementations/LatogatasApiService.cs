using MadarfigyeloApp.API;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;


namespace MadarfigyeloApp.Implementations
{
    public class LatogatasApiService : ApiServiceBase, ILatogatasApiService
    {
        private readonly IGenericApi<Latogatas> _latogatasApi;
        public LatogatasApiService(
            IGenericApi<Latogatas> latogatasApi,
            INavigationService navigation) : base(navigation)
        {
            _latogatasApi = latogatasApi ?? throw new ArgumentNullException(nameof(latogatasApi));
        }

        public async Task<List<Latogatas>> GetAllLatogatasAsync(bool forceRefresh = false)
        {
            var response = await _latogatasApi.GetAllAsync(forceRefresh ? "true" : "false");
            await HandleResponseAsync(response);
            return response.Content ?? [];
        }

        public async Task<Latogatas?> GetLatogatasAsync(int id)
        {
            var response = await _latogatasApi.GetAsync(id);
            await HandleResponseAsync(response);
            return response.Content;
        }

        public async Task<List<Latogatas>> GetLatogatasByOduAsync(int oduId, bool forceRefresh = false)
        {
            var response = await _latogatasApi.GetByParentAsync(oduId, forceRefresh ? "true" : "false");
            await HandleResponseAsync(response);
            return response.Content ?? [];
        }

        public async Task<bool> UpdateLatogatas(Latogatas latogatas)
        {
            var response = await _latogatasApi.PutAsync(latogatas.Id, latogatas);
            if (await HandleResponseAsync(response))
            {
                InvalidateCache(latogatas, ModifiedAction.Updated);
                return true;
            }
            return false;
        }

        public async Task<bool> PostLatogatasAsync(Latogatas latogatas)
        {
            var response = await _latogatasApi.PostAsync(latogatas);
            if (await HandleResponseAsync(response))
            {
                InvalidateCache(latogatas, ModifiedAction.Created);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateLatogatasAsync(Latogatas latogatas)
        {
           var response = await _latogatasApi.PutAsync(latogatas.Id, latogatas);
            if (response.IsSuccessStatusCode)
            {
                InvalidateCache(latogatas, ModifiedAction.Updated);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteLatogatasAsync(int id)
        {
            var response = await _latogatasApi.DeleteAsync(id);
            if (response.IsSuccessStatusCode)
            {
                InvalidateCache(new Latogatas { Id = id }, ModifiedAction.Deleted);
                return true;
            }
            return false;
        }
    }
}

using MadarfigyeloApp.API;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using static MadarfigyeloApp.Implementations.OdutelepApiService;

namespace MadarfigyeloApp.Implementations
{
    public class OduApiService : ApiServiceBase, IOduApiService
    {
        private readonly IGenericApi<Odu> _oduApi;
        public OduApiService(
            IGenericApi<Odu> oduApi,
            INavigationService navigation) : base(navigation)
        {
            _oduApi = oduApi ?? throw new ArgumentNullException(nameof(oduApi));
        }

        public async Task<List<Odu>> GetAllOduAsync(bool forceRefresh = false)
        {
            var response = await _oduApi.GetAllAsync(forceRefresh ? "true" : "false");
            await HandleResponseAsync(response);
            return response.Content ?? [];
        }

        public async Task<Odu?> GetOduAsync(int id)
        {
            var response = await _oduApi.GetAsync(id);
            await HandleResponseAsync(response);
            return response.Content;
        }

        public async Task<List<Odu>> GetOduByOdutelepAsync(int odutelepId, bool forceRefresh = false)
        {
            var response = await _oduApi.GetByParentAsync(odutelepId, forceRefresh ? "true" : "false");
            await HandleResponseAsync(response);
            return response.Content ?? [];
        }
        public async Task<bool> PostOduAsync(Odu odu)
        {
            var response = await _oduApi.PostAsync(odu);
            if (await HandleResponseAsync(response))
            {
                InvalidateCache(odu, ModifiedAction.Created);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateOduAsync(Odu odu)
        {
            var response = await _oduApi.PutAsync(odu.Id, odu);
            if (await HandleResponseAsync(response))
            {
                InvalidateCache(odu, ModifiedAction.Updated);
                return true;
            }
            return false;
        }
    }
}

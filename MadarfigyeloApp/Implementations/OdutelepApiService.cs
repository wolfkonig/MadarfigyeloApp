using Terepnaplo.API;
using Terepnaplo.Contracts;
using Terepnaplo.Models;

namespace Terepnaplo.Implementations
{
    public partial class OdutelepApiService : ApiServiceBase, IOdutelepApiService
    {
        private readonly IGenericApi<Odutelep> _odutelepApi;

        public OdutelepApiService(
            IGenericApi<Odutelep> odutelepApi,
            INavigationService navigation) : base(navigation)
        {
            _odutelepApi = odutelepApi ?? throw new ArgumentNullException(nameof(odutelepApi));
        }

        public async Task<List<Odutelep>> GetAllOdutelepAsync(bool forceRefresh = false)
        {
            var response = await _odutelepApi.GetAllAsync(forceRefresh ? "true" : "false");
            await HandleResponseAsync(response);
            return response.Content ?? [];
        }    

        public async Task<Odutelep?> GetOdutelepAsync(int id)
        {
            var response = await _odutelepApi.GetAsync(id);
            await HandleResponseAsync(response);
            return response.Content;
        }       

        public async Task<bool> PostOdutelepAsync(Odutelep odutelep)
        {
            var response = await _odutelepApi.PostAsync(odutelep);
            if (await HandleResponseAsync(response))
            {
                InvalidateCache(odutelep, ModifiedAction.Created);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateOdutelep(Odutelep odutelep)
        {
            var response = await _odutelepApi.PutAsync(odutelep.Id, odutelep);
            if (await HandleResponseAsync(response))
            {
                InvalidateCache(odutelep, ModifiedAction.Updated);
                return true;
            }
            return false;
        }
    }
}

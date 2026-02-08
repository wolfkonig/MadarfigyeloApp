using MadarfigyeloApp.API;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.Implementations
{
    public class ApiService : IApiService
    {
        private readonly IOdutelepApi _odutelepApi;
        private readonly IOduApi _oduApi;
        private readonly ILatogatasApi _latogatasApi;

        public ApiService(IOdutelepApi odutelepApi, IOduApi oduApi, ILatogatasApi latogatasApi) 
        { 
            _odutelepApi = odutelepApi ?? throw new ArgumentNullException(nameof(odutelepApi));
            _oduApi = oduApi ?? throw new ArgumentNullException(nameof(oduApi));
            _latogatasApi = latogatasApi ?? throw new ArgumentNullException(nameof(latogatasApi));
        }

        public async Task<List<Latogatas>> GetAllLatogatasAsync()
        {
            var result = await _latogatasApi.GetAllLatogatasAsync();
            return result.Content ?? [];
        }

        public async Task<List<Odu>> GetAllOduAsync()
        {
            var result = await _oduApi.GetAllOduAsync();
            return result.Content ?? [];
        }

        public async Task<List<Odutelep>> GetAllOdutelepAsync()
        {
            var result = await _odutelepApi.GetAllOdutelepAsync();
            return result.Content ?? [];
        }

        public async Task<Latogatas> GetLatogatasAsync(int id)
        {
            var result = await _latogatasApi.GetLatogatasAsync(id);
            return result.Content;
        }

        public async Task<Odu> GetOduAsync(int id)
        {
            var result = await _oduApi.GetOduAsync(id);
            return result.Content;
        }

        public async Task<Odutelep> GetOdutelepAsync(int id)
        {
            var result = await _odutelepApi.GetOdutelepAsync(id);
            return result.Content;
        }

        public async Task PostLatogatasAsync(Latogatas latogatas)
        {
            var result = await _latogatasApi.PostLatogatasAsync(latogatas);
        }

        public async Task PostOduAsync(Odu odu)
        {
            var result = await _oduApi.PostOduAsync(odu);
        }

        public async Task PostOdutelepAsync(Odutelep odutelep)
        {
            var result = await _odutelepApi.PostOdutelepAsync(odutelep);
        }
    }
}

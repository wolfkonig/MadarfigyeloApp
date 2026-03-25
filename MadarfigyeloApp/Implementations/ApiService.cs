using MadarfigyeloApp.API;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Resources;
using Refit;
using System.Text;

namespace MadarfigyeloApp.Implementations
{
    public class ApiService : IApiService
    {
        private readonly IGenericApi<Odutelep> _odutelepApi;
        private readonly IGenericApi<Odu> _oduApi;
        private readonly IGenericApi<Latogatas> _latogatasApi;
        private readonly INavigationService _navigation;

        public ApiService(
            IGenericApi<Odutelep> odutelepApi,
            IGenericApi<Odu> oduApi,
            IGenericApi<Latogatas> latogatasApi,
            INavigationService navigation)
        {
            _odutelepApi = odutelepApi ?? throw new ArgumentNullException(nameof(odutelepApi));
            _oduApi = oduApi ?? throw new ArgumentNullException(nameof(oduApi));
            _latogatasApi = latogatasApi ?? throw new ArgumentNullException(nameof(latogatasApi));
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
        }

        public async Task<List<Latogatas>> GetAllLatogatasAsync(bool forceRefresh = false)
        {
            var response = await _latogatasApi.GetAllAsync(forceRefresh ? "true" : "false");
            await HandleResponseAsync(response);
            return response.Content ?? [];
        }

        public async Task<List<Odu>> GetAllOduAsync(bool forceRefresh = false)
        {
            var response = await _oduApi.GetAllAsync(forceRefresh ? "true" : "false");
            await HandleResponseAsync(response);
            return response.Content ?? [];
        }

        public async Task<List<Odutelep>> GetAllOdutelepAsync(bool forceRefresh = false)
        {
            var response = await _odutelepApi.GetAllAsync(forceRefresh ? "true" : "false");
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

        public async Task<Odutelep?> GetOdutelepAsync(int id)
        {
            var response = await _odutelepApi.GetAsync(id);
            await HandleResponseAsync(response);
            return response.Content;
        }

        public async Task<bool> PostLatogatasAsync(Latogatas latogatas)
        {
            var response = await _latogatasApi.PostAsync(latogatas);
            return await HandleResponseAsync(response);
        }

        public async Task<bool> PostOduAsync(Odu odu)
        {
            var response = await _oduApi.PostAsync(odu);
            return await HandleResponseAsync(response);
        }

        public async Task<bool> PostOdutelepAsync(Odutelep odutelep)
        {
            var response = await _odutelepApi.PostAsync(odutelep);
            return await HandleResponseAsync(response);
        }

        public async Task<bool> UpdateOduAsync(Odu odu)
        {
            var response = await _oduApi.PutAsync(odu.Id, odu);
            return await HandleResponseAsync(response);
        }

        public async Task<bool> UpdateOdutelep(Odutelep odutelep)
        {
            var response = await _odutelepApi.PutAsync(odutelep.Id, odutelep);
            return await HandleResponseAsync(response);
        }

        public async Task<bool> UpdateLatogatas(Latogatas latogatas)
        {
            var response = await _latogatasApi.PutAsync(latogatas.Id, latogatas);
            return await HandleResponseAsync(response);
        }

        private async Task<bool> HandleResponseAsync(IApiResponse apiResponse)
        {
            if(apiResponse.IsSuccessful)
            {
                return true;
            }

            var message = new StringBuilder();
            message.AppendLine(AppRes.ApiError);

            switch (apiResponse.StatusCode)
            {
                case System.Net.HttpStatusCode.Unauthorized:
                    message.AppendLine(AppRes.ApiErrorAuth);
                    break;
                case System.Net.HttpStatusCode.RequestTimeout:
                    message.AppendLine(AppRes.ApiErrorTimeout);
                    break;
                case System.Net.HttpStatusCode.BadRequest:
                    message.AppendLine(AppRes.ApiErrorBadReq);
                    break;
                default:
                    message.AppendLine(apiResponse.Error.Message);
                    break;
            }
                    
            await _navigation.ShowAlertAsync(message.ToString());
            return false;
        }
    }
}

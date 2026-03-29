using MadarfigyeloApp.API;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Resources;
using MonkeyCache.FileStore;
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
            InvalidateCache(latogatas, ModifiedAction.Created);
            return await HandleResponseAsync(response);
        }

        public async Task<bool> PostOduAsync(Odu odu)
        {
            var response = await _oduApi.PostAsync(odu);
            InvalidateCache(odu, ModifiedAction.Created);
            return await HandleResponseAsync(response);
        }

        public async Task<bool> PostOdutelepAsync(Odutelep odutelep)
        {
            var response = await _odutelepApi.PostAsync(odutelep);
            InvalidateCache(odutelep, ModifiedAction.Created);
            return await HandleResponseAsync(response);
        }

        public async Task<bool> UpdateOduAsync(Odu odu)
        {
            var response = await _oduApi.PutAsync(odu.Id, odu);
            InvalidateCache(odu, ModifiedAction.Updated);
            return await HandleResponseAsync(response);
        }

        public async Task<bool> UpdateOdutelep(Odutelep odutelep)
        {
            var response = await _odutelepApi.PutAsync(odutelep.Id, odutelep);
            InvalidateCache(odutelep, ModifiedAction.Updated);
            return await HandleResponseAsync(response);
        }

        public async Task<bool> UpdateLatogatas(Latogatas latogatas)
        {
            var response = await _latogatasApi.PutAsync(latogatas.Id, latogatas);
            InvalidateCache(latogatas, ModifiedAction.Updated);
            return await HandleResponseAsync(response);
        }

        private async Task<bool> HandleResponseAsync(IApiResponse apiResponse)
        {
            if (apiResponse.IsSuccessful)
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

        private static void InvalidateCache(object modifiedOject, ModifiedAction action)
        {
            try
            {
                switch (modifiedOject)
                {
                    case Odutelep ot when action is ModifiedAction.Created or ModifiedAction.Deleted:
                        Barrel.Current.Empty(
                            $"{Constants.BaseUrlHttp}/odutelep", 
                            $"{Constants.BaseUrlHttp}/odu/ByParent/{ot.Id}");
                        break;
                    case Odu o when action is ModifiedAction.Created or ModifiedAction.Deleted:
                        Barrel.Current.Empty(
                            $"{Constants.BaseUrlHttp}/odu", 
                            $"{Constants.BaseUrlHttp}/odu/ByParent/{o.OdutelepId}", 
                            $"{Constants.BaseUrlHttp}/latogatas/ByParent/{o.Id}");
                        break;
                    case Latogatas l when action is ModifiedAction.Created or ModifiedAction.Deleted:
                        Barrel.Current.Empty(
                            $"{Constants.BaseUrlHttp}/latogatas", 
                            $"{Constants.BaseUrlHttp}/latogatas/ByParent/{l.OduId}");
                        break;
                    case Odu o1 when action is ModifiedAction.Updated:
                        Barrel.Current.Empty(
                            $"{Constants.BaseUrlHttp}/odu",
                            $"{Constants.BaseUrlHttp}/odu/{o1.Id}", 
                            $"{Constants.BaseUrlHttp}/odu/ByParent/{o1.OdutelepId}");
                        break;
                    case Odutelep ot1 when action is ModifiedAction.Updated:
                        Barrel.Current.Empty(
                            $"{Constants.BaseUrlHttp}/odutelep",
                            $"{Constants.BaseUrlHttp}/odu/ByParent/{ot1.Id}");
                        break;
                    case Latogatas l1 when action is ModifiedAction.Updated:
                        Barrel.Current.Empty(
                            $"{Constants.BaseUrlHttp}/latogatas",
                            $"{Constants.BaseUrlHttp}/latogatas/{l1.Id}",
                            $"{Constants.BaseUrlHttp}/latogatas/ByParent/{l1.OduId}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cache invalidation failed: {ex.Message}");
            }
        }

        public enum ModifiedAction
        {
            Created,
            Updated,
            Deleted
        }
    }
}

using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.Resources;
using MonkeyCache.FileStore;
using Refit;
using System.Text;
using static MadarfigyeloApp.Implementations.OdutelepApiService;

namespace MadarfigyeloApp.Implementations
{
    public abstract class ApiServiceBase
    {
        protected readonly INavigationService _navigation;

        public ApiServiceBase(INavigationService navigationService)
        {
            _navigation = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        protected static void InvalidateCache(object modifiedOject, ModifiedAction action)
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

        protected async Task<bool> HandleResponseAsync(IApiResponse apiResponse)
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
    }
}

using Refit;

namespace MadarfigyeloApp.API
{
    public interface IGenericApi<T> where T : class
    {
        [Get("/{id}")]
        Task<IApiResponse<T>> GetAsync(int id);

        [Get("")]
        Task<IApiResponse<List<T>>> GetAllAsync([Header("X-Force-Refresh")] string? forceRefresh = null);

        [Get("/ByParent/{parentId}")]
        Task<IApiResponse<List<T>>> GetByParentAsync(int parentId, [Header("X-Force-Refresh")] string? forceRefresh = null);

        [Post("")]
        Task<IApiResponse> PostAsync([Body] T payload);

        [Put("/{id}")]
        Task<IApiResponse> PutAsync(int id, [Body] T payload);
    }
}

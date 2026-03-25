namespace MadarfigyeloApp.Contracts
{
    public interface ISettingsService
    {
        int SelectedOdutelepId { get; set; }
        int SelectedOduId { get; set; }
        bool ForceRefresh { get; set; }
        void Set<T>(string key, T value);
        T? Get<T>(string key, T? defaultValue = default);
        void Remove(string key);
        void RemoveAll();
        Task<string?> SecureGet(string key);
        Task SecureSet(string key, string value);
    }
}

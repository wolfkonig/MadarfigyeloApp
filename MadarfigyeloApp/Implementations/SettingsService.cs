using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Models;

namespace MadarfigyeloApp.Implementations
{
    public class SettingsService : ISettingsService
    {
        public int SelectedOdutelepId 
        {
            get => Get(Constants.KeySelectedOdutelepId, Odutelep.Empty.Id);
            set => Set(Constants.KeySelectedOdutelepId, value);
        }

        public int SelectedOduId 
        { 
            get => Get(Constants.KeySelectedOduId, Odu.Empty.Id); 
            set => Set(Constants.KeySelectedOduId, value ); 
        }

        public bool ForceRefresh
        {
            get => Get(Constants.KeyForceRefresh, false);
            set => Set(Constants.KeyForceRefresh, value);
        }

        public T? Get<T>(string key, T? defaultValue = default)
        {
            return Preferences.Default.Get(key, defaultValue);
        }

        public void Set<T>(string key, T value)
        {
            Preferences.Default.Set(key, value);
        }

        public Task<string?> SecureGet(string key)
        {
             return SecureStorage.Default.GetAsync(key);
        }

        public Task SecureSet(string key, string value)
        {
            return SecureStorage.Default.SetAsync(key, value);
        }

        public void Remove(string key)
        {
            Preferences.Default.Remove(key);
            SecureStorage.Remove(key);
        }

        public void RemoveAll()
        {
            Preferences.Default.Clear();
            SecureStorage.Default.RemoveAll();
        }
    }
}

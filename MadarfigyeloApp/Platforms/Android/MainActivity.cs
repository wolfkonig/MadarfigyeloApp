using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Activity;

namespace MadarfigyeloApp.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            OnBackPressedDispatcher.AddCallback(new ShellBackPressedCallback(true));
        }
    }

    /// <summary>
    /// Override the default back button behavior to pop the navigation stack when there are pages to go back to, otherwise do nothing
    /// </summary>
    /// <param name="enabled"></param>
    internal class ShellBackPressedCallback(bool enabled) : OnBackPressedCallback(enabled)
    {
        private bool enabled = enabled;

        public override void HandleOnBackPressed()
        {
            if (enabled)
            {
                if (Shell.Current.Navigation.NavigationStack.Count > 1)
                {
                    Task.Run(async () => await Shell.Current.Navigation.PopAsync());
                }            
            }
        }
    }

}

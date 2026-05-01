using Terepnaplo;

namespace Terepnaplo;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // Create a fresh Shell for this window
        var shell = new AppShell();
        var window = new Window(shell);

        // Ensure we only do initial navigation once per window
        bool didInitialNav = false;

        // Navigate after the window is created (UI is ready)
        window.Created += async (s, e) =>
        {
            if (didInitialNav) return;
            didInitialNav = true;

            var loggedInUser = Preferences.Default.Get<string?>(Constants.KeyLoggedInUserEmail, null);

            if (loggedInUser != null) return;

            // Dispatch to UI thread to ensure handlers are ready
            Dispatcher.Dispatch(async () =>
            {
                // Start the app on the login route and clear back stack
                await shell.GoToAsync($"//{Constants.RouteLogin}");
            });
        };

        return window;
    }
}

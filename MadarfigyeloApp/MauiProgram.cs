using CommunityToolkit.Maui;
using MadarfigyeloApp.API;
using MadarfigyeloApp.Contracts;
using MadarfigyeloApp.Implementations;
using MadarfigyeloApp.Models;
using MadarfigyeloApp.ViewModels;
using MadarfigyeloApp.Views;
using Microsoft.Extensions.Logging;
using Refit;

namespace MadarfigyeloApp
{
    public static class MauiProgram
    {
        public static Environment CurrentEnvironment { get; } = Environment.Dev;

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .RegisterClients()
                .RegisterServices()
                .RegisterViewModels();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<OdutelepViewModel>();
            builder.Services.AddTransient<OduViewModel>();
            builder.Services.AddTransient<LatogatasViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<OduMapViewModel>();

            builder.Services.AddTransientWithShellRoute<NewOdutelepView, NewOdutelepViewModel>(Constants.RouteNewOdutelep);
            builder.Services.AddTransientWithShellRoute<NewOduView, NewOduViewModel>(Constants.RouteNewOdu);
            builder.Services.AddTransientWithShellRoute<NewLatogatasView, NewLatogatasViewModel>(Constants.RouteNewLatogatas);

            return builder;
        }

        private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerService, ConsoleLogger>();
            builder.Services.AddSingleton<INavigationService, ShellNavigationService>();
            builder.Services.AddSingleton<IApiService, ApiService>();
            builder.Services.AddSingleton<ILocationService, LocationService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            return builder;
        }

        private static MauiAppBuilder RegisterClients(this MauiAppBuilder builder)
        {
            var refitSettings = new RefitSettings(new NewtonsoftJsonContentSerializer());

            if (CurrentEnvironment == Environment.DevLocal)
            {
                builder.Services.AddTransient<TokenAuthHandler>();
                var acceptAllClientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (_, _, _, _) => true,                    
                };

                builder.Services.AddRefitClient<IAuthApi>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.LocalBaseUrlHttps))
                    .ConfigurePrimaryHttpMessageHandler(() => acceptAllClientHandler);

                builder.Services.AddRefitClient<IGenericApi<Odutelep>>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.LocalBaseUrlHttps + "/Odutelep"))
                    .ConfigurePrimaryHttpMessageHandler(() => acceptAllClientHandler)
                    .AddHttpMessageHandler<TokenAuthHandler>();

                builder.Services.AddRefitClient<IGenericApi<Odu>>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.LocalBaseUrlHttps + "/Odu"))
                    .ConfigurePrimaryHttpMessageHandler(() => acceptAllClientHandler)
                    .AddHttpMessageHandler<TokenAuthHandler>();

                builder.Services.AddRefitClient<IGenericApi<Latogatas>>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.LocalBaseUrlHttps + "/Latogatas"))
                    .ConfigurePrimaryHttpMessageHandler(() => acceptAllClientHandler)
                    .AddHttpMessageHandler<TokenAuthHandler>();
      
            }
            else if (CurrentEnvironment == Environment.Dev)
            {
                builder.Services.AddTransient<TokenAuthHandler>();

                builder.Services.AddRefitClient<IGenericApi<Odutelep>>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.BaseUrlHttp + "/Odutelep"))
                    .AddHttpMessageHandler<TokenAuthHandler>();

                builder.Services.AddRefitClient<IGenericApi<Odu>>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.BaseUrlHttp + "/Odu"))
                    .AddHttpMessageHandler<TokenAuthHandler>();

                builder.Services.AddRefitClient<IGenericApi<Latogatas>>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.BaseUrlHttp + "/Latogatas"))
                    .AddHttpMessageHandler<TokenAuthHandler>();

                builder.Services.AddRefitClient<IAuthApi>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.BaseUrlHttp));
            }
            else
            {
                throw new NotImplementedException();
            }
            return builder;
        }
    }

}

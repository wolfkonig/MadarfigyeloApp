using CommunityToolkit.Maui;
using MadarfigyeloApp.API;
using MadarfigyeloApp.Services;
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

            builder.Services.AddTransientWithShellRoute<NewOdutelepView, NewOdutelepViewModel>(Constants.RouteNewOdutelep);
            builder.Services.AddTransientWithShellRoute<NewOduView, NewOduViewModel>(Constants.RouteNewOdu);
            builder.Services.AddTransientWithShellRoute<NewLatogatasView, NewLatogatasViewModel>(Constants.RouteNewLatogatas);

            return builder;
        }

        private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<INavigationService, ShellNavigationService>();
            return builder;
        }

        private static MauiAppBuilder RegisterClients(this MauiAppBuilder builder)
        {
            var refitSettings = new RefitSettings(new NewtonsoftJsonContentSerializer());

            if (CurrentEnvironment == Environment.DevLocal)
            {
                var handler = new HttpClientHandler
                {
                    // http client handler for DEBUG only - accepts any certificate
                    ServerCertificateCustomValidationCallback = (_, _, _, _) => true
                };
                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(Constants.LocalBaseUrlHttps),
                };
                builder.Services.AddSingleton(RestService.For<IOdutelepApi>(httpClient, refitSettings));
                builder.Services.AddSingleton(RestService.For<IOduApi>(httpClient, refitSettings));
                builder.Services.AddSingleton(RestService.For<ILatogatasApi>(httpClient, refitSettings));
            }
            else if (CurrentEnvironment == Environment.Dev)
            {
                builder.Services.AddTransient<BasicAuthHandler>();

                var baseUri = new Uri(Constants.BaseUrlHttp);

                builder.Services.AddRefitClient<IOdutelepApi>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = baseUri)
                    .AddHttpMessageHandler<BasicAuthHandler>();
                builder.Services.AddRefitClient<IOduApi>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = baseUri)
                    .AddHttpMessageHandler<BasicAuthHandler>();
                builder.Services.AddRefitClient<ILatogatasApi>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = baseUri)
                    .AddHttpMessageHandler<BasicAuthHandler>();
            }
            else
            {
                throw new NotImplementedException();
            }
            return builder;
        }
    }

}

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
        public static Environment CurrentEnvironment { get; } = Environment.DevLocal;

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
            builder.Services.AddTransient<LoginViewModel>();

            builder.Services.AddTransientWithShellRoute<NewOdutelepView, NewOdutelepViewModel>(Constants.RouteNewOdutelep);
            builder.Services.AddTransientWithShellRoute<NewOduView, NewOduViewModel>(Constants.RouteNewOdu);
            builder.Services.AddTransientWithShellRoute<NewLatogatasView, NewLatogatasViewModel>(Constants.RouteNewLatogatas);

            return builder;
        }

        private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
        {
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
                static HttpClient GetHttpClient(string controllerUri)
                {
                    var handler = new HttpClientHandler
                    {
                        // http client handler for LOCAL DEBUG only - accepts any certificate
                        ServerCertificateCustomValidationCallback = (_, _, _, _) => true
                    };
                    return new HttpClient(handler)
                    {
                        BaseAddress = new Uri(Constants.LocalBaseUrlHttps + controllerUri),
                        Timeout = TimeSpan.FromSeconds(5)
                    };
                }                

                builder.Services.AddSingleton(RestService.For<IGenericApi<Odutelep>>(GetHttpClient("/Odutelep"), refitSettings));
                builder.Services.AddSingleton(RestService.For<IGenericApi<Odu>>(GetHttpClient("/Odu"), refitSettings));
                builder.Services.AddSingleton(RestService.For<IGenericApi<Latogatas>> (GetHttpClient("/Latogatas"), refitSettings));
                builder.Services.AddSingleton(RestService.For<IAuthApi>(GetHttpClient(""), refitSettings));
            }
            else if (CurrentEnvironment == Environment.Dev)
            {
                builder.Services.AddTransient<BasicAuthHandler>();

                builder.Services.AddRefitClient<IGenericApi<Odutelep>>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.BaseUrlHttp + "/Odutelep"))
                    .AddHttpMessageHandler<BasicAuthHandler>();

                builder.Services.AddRefitClient<IGenericApi<Odu>>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.BaseUrlHttp + "/Odu"))
                    .AddHttpMessageHandler<BasicAuthHandler>();

                builder.Services.AddRefitClient<IGenericApi<Latogatas>>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.BaseUrlHttp + "/Latogatas"))
                    .AddHttpMessageHandler<BasicAuthHandler>();

                builder.Services.AddRefitClient<IAuthApi>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.BaseUrlHttp))
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

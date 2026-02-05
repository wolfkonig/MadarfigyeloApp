using CommunityToolkit.Maui;
using MadarfigyeloApp.API;
using MadarfigyeloApp.ViewModels;
using MadarfigyeloApp.Views;
using Microsoft.Extensions.Logging;
using Refit;

namespace MadarfigyeloApp
{
    public static class MauiProgram
    {
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
                .RegisterServices()
                .RegisterViewModels()
                .RegisterViews();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<OdutelepView>();
            return builder;
        }

        private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<OdutelepViewModel>();
            builder.Services.AddTransient<OduViewModel>();
            builder.Services.AddTransient<LatogatasViewModel>();
            return builder;
        }

        private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
        {
            //var handler = new HttpClientHandler
            //{
            //    // http client handler for DEBUG only - accepts any certificate
            //    ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            //};
            //var httpClient = new HttpClient(handler)
            //{
            //    BaseAddress = new Uri(Constants.BaseUrlHttp),
            //};
            //builder.Services.AddSingleton(RestService.For<IOdutelepApi>(httpClient));
            //builder.Services.AddSingleton(RestService.For<IOduApi>(httpClient));
            //builder.Services.AddSingleton(RestService.For<ILatogatasApi>(httpClient));

            builder.Services.AddTransient<BasicAuthHandler>();

            builder.Services.AddRefitClient<IOdutelepApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.BaseUrlHttp))
                .AddHttpMessageHandler<BasicAuthHandler>();
            builder.Services.AddRefitClient<IOduApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.BaseUrlHttp))
                .AddHttpMessageHandler<BasicAuthHandler>();
            builder.Services.AddRefitClient<ILatogatasApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.BaseUrlHttp))
                .AddHttpMessageHandler<BasicAuthHandler>();

            return builder;
        }
    }

}

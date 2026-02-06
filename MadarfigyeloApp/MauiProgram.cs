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
            builder.Services.AddTransient<NewOdutelepViewModel>();
            builder.Services.AddTransient<NewOduViewModel>();
            builder.Services.AddTransient<NewLatogatasViewModel>();


            builder.Services.AddTransientWithShellRoute<NewOdutelepView, NewOdutelepViewModel>(Constants.RouteNewOdutelep);
            builder.Services.AddTransientWithShellRoute<NewOduView, NewOduViewModel>(Constants.RouteNewOdu);
            builder.Services.AddTransientWithShellRoute<NewLatogatasView, NewLatogatasViewModel>(Constants.RouteNewLatogatas);

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
            builder.Services.AddSingleton<INavigationService,ShellNavigationService>();

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

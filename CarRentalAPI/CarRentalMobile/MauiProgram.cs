using Microsoft.Extensions.Logging;
using CommunityToolkit.Mvvm;

using CarRentalMobile.Services; 
using CarRentalMobile.ViewModels; 
using CarRentalMobile.Views; 

namespace CarRentalMobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();

#endif

            // singleton - jedna instancja na cala apke
            builder.Services.AddSingleton<CarRentalApiService>();

            // transient - zawsze nowa instancja gdy jest potrzebna
            builder.Services.AddTransient<CitiesViewModel>();

            builder.Services.AddTransient<CitiesPage>();

            builder.Services.AddTransient<CarsViewModel>(); 
            builder.Services.AddTransient<CarsPage>();

            //builder.Services.AddSingleton<MainPage>();


            return builder.Build();
        }
    }
}

using Microsoft.Extensions.Logging;
using PersonalBookLibrary.Pages;
using PersonalBookLibrary.Services;
using PersonalBookLibrary.ViewModels;

namespace PersonalBookLibrary
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
                    fonts.AddFont("Roboto-Regular.ttf", "FontFamilyRegular");
                    fonts.AddFont("Roboto-Medium.ttf", "FontFamilySemibold");
                });

            // Pages
            builder.Services.AddTransient<BookListPage>();
            builder.Services.AddTransient<NewBookPage>();

            // Viewmodels
            builder.Services.AddTransient<BookListViewModel>();
            builder.Services.AddTransient<NewBookViewModel>();

            // Services
            builder.Services.AddScoped<IPlatformService, PlatformService>();
            builder.Services.AddScoped<IFileSystemService, FileSystemService>();
            builder.Services.AddScoped<IPreferencesService, PreferencesService>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

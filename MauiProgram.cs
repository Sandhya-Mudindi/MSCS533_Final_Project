using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Maps;
using LocationTracking.Services;
using System.IO;


namespace LocationTracking;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiMaps() // Important for using Microsoft.Maui.Controls.Maps
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "locations.db3");
        builder.Services.AddSingleton(new DatabaseService(dbPath));

        // Register Pages
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<HeatMapPage>();

     
        return builder.Build();
    }
}

using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace AlienCyborgSynergyNetwork
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
                });

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "synergy.db");
            builder.Services.AddDbContext<SynergyDBContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Directory.CreateDirectory(folder);
            var dbPath = Path.Combine(folder, "synergy.db");

            builder.Services.AddDbContext<SynergyDBContext>(opts =>
                opts.UseSqlite($"Data Source={dbPath}"));

            builder.Services.AddScoped<SynergyDBContextServices>();
            builder.Services.AddScoped<AuthenticatingService>();
            builder.Services.AddSingleton<SessionState>();
            builder.Services.AddMauiBlazorWebView();

    #if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
    #endif

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<SynergyDBContext>();

            ctx.Database.Migrate();

            return app;
        }
    }
}
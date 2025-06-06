﻿using System;
using System.IO;
using System.Linq;
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

            var dbPathCyborgSession = Path.Combine(folder, "cyborg.db");
            builder.Services.AddDbContext<CyborgSessionDBContext>(opts =>
                opts.UseSqlite($"Data Source={dbPathCyborgSession}"));

            builder.Services.AddScoped<SynergyDBContextServices>();
            builder.Services.AddScoped<AuthenticatingService>();
            builder.Services.AddSingleton<SessionState>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                // a) SynergyDBContext
                var synergyCtx = scope.ServiceProvider.GetRequiredService<SynergyDBContext>();
                if (synergyCtx.Database.GetPendingMigrations().Any())
                {
                    synergyCtx.Database.Migrate();
                }

                // b) CyborgSessionDBContext
                var cyborgCtx = scope.ServiceProvider.GetRequiredService<CyborgSessionDBContext>();
                if (cyborgCtx.Database.GetPendingMigrations().Any())
                {
                    cyborgCtx.Database.Migrate();
                }
            }

            return app;
        }
    }
}
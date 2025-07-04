﻿using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Client;
using AlienCyborgSynergyNetwork.Shared;
using MediatR;

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

            // Registering the DbContexts
            var dbPath = Path.Combine(folder, "synergy.db");
            builder.Services.AddDbContext<SynergyDBContext>(opts =>
                opts.UseSqlite($"Data Source={dbPath}"));

            var dbPathCyborgSession = Path.Combine(folder, "cyborg.db");
            builder.Services.AddDbContext<CyborgSessionDBContext>(opts =>
                opts.UseSqlite($"Data Source={dbPathCyborgSession}"));

            var dbPathFirmware = Path.Combine(folder, "firmware.db");
            builder.Services.AddDbContext<FirmwareDBContext>(opts =>
                opts.UseSqlite($"Data Source={dbPathFirmware}"));

            var dbPathSensor = Path.Combine(folder, "sensor.db");
            builder.Services.AddDbContext<SensorDBContext>(opts =>
            opts.UseSqlite($"Data Source={dbPathSensor}"));

            // Registering the repositories and services
            builder.Services.AddScoped<SynergyDBContextServices>();
            builder.Services.AddScoped<AuthenticatingService>();
            builder.Services.AddSingleton<SessionState>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISensorUnitOfWork, SensorUnitOfWork>();
            builder.Services.AddScoped<IFirmwareUnitOfWork, FirmwareUnitOfWork>();
            builder.Services.AddScoped<IFirmwareRepository, FirmwareRepository>();
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<HubConnection>(sp =>
                new HubConnectionBuilder()
                    .WithUrl("https://localhost:7142/neuralhub")
                    .WithAutomaticReconnect()
                    .Build());
            builder.Services.AddScoped(sp =>
            new HttpClient
            {
                BaseAddress = new Uri("http://192.168.0.179:5001")
            });
            
            builder.Services.AddMediatR(cfg =>
                 cfg.RegisterServicesFromAssemblyContaining<SubmitRatingHandler>());

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
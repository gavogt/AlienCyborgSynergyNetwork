using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.Design;
using Hubs;
using AlienCyborgSynergyNetwork;
using System;
using AlienCyborgSynergyNetwork.Shared;

var builder = WebApplication.CreateBuilder(args);
var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
Directory.CreateDirectory(folder);

var dbPathSensor = Path.Combine(folder, "sensor.db");
builder.Services.AddDbContext<SensorDBContext>(opts =>
opts.UseSqlite($"Data Source={dbPathSensor}"));

builder.Services.AddScoped<ISensorUnitOfWork, SensorUnitOfWork>();
builder.Services.AddSignalR();
builder.Services.AddHostedService<MqttSubscriberService>();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    scope.ServiceProvider
        .GetRequiredService<SensorDBContext>()
        .Database.Migrate();
}

app.MapGet("/", () => "SignalR server is running");
app.MapHub<NeuralHub>("/neuralhub");

app.Run("https://localhost:7142");

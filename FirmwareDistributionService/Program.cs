using AlienCyborgSynergyNetwork.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.Design;
using RabbitMQ.Client;
using FirmwareDistributionService;
using Microsoft.AspNetCore.SignalR;
using Hubs;
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://192.168.0.179:5001");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
var dbPath = Path.Combine(folder, "firmware.db");
builder.Services.AddDbContext<FirmwareDBContext>(opts =>
    opts.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddScoped<IFirmwareUnitOfWork, FirmwareUnitOfWork>();
builder.Services.AddSingleton<JobPublisher>();
builder.Services.AddTransient<PushCommand>();
builder.Services.AddTransient<SignCommand>();
builder.Services.AddHostedService<FirmwareJobProcessor>();
builder.Services.AddSignalR();

builder.Services.AddSingleton<IEnumerable<ICommand>>(sp => new ICommand[] {
    sp.GetRequiredService<SignCommand>(),
    sp.GetRequiredService<PushCommand>()
});



var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();
app.MapHub<NeuralHub>("/neuralhub");
app.MapControllers();
app.MapGet("/", () => "Firmware Server Running");

app.Run();

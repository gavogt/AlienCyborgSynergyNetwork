using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://192.168.0.179:5001");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Firmware Server Running");

app.Run();

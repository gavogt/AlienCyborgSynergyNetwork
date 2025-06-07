using NeuralHub;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
var app = builder.Build();

app.MapHub<StreamingHub>("/hubs/stream");

app.Run();

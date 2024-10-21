using SurvivalApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/hello", () => "Hello World!");

app.MapHub<GameHub>("/api/hub");

app.Run();

using SurvivalApi.Hubs;
using SurvivalApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/hello", () => "Hello World!");

app.MapGet("/api/test/ollama", async () => { 
    var ollamaService = new OllamaService();
    await ollamaService.TestSendToOllama();
});

app.MapGet("/api/test/create", async () => { 
    var ollamaService = new OllamaService();
    await ollamaService.TestCreateScenario();
});

app.MapGet("/api/test/run", async () => { 
    var ollamaService = new OllamaService();
    await ollamaService.TestRunScenario();
});

app.MapHub<GameHub>("/api/hub");

app.Run();

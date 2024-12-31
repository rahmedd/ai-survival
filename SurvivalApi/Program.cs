using StackExchange.Redis;
using api.Hubs;
using api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

var redisConnectionString = builder.Configuration["ConnectionStrings:REDIS_CONNECTION_STRING"] ?? throw new ArgumentNullException("REDIS_CONNECTION_STRING", "Connection string 'REDIS_CONNECTION_STRING' not found.");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

// var connectionString = builder.Configuration.GetValue<string>("DB_CONNECTION_STRING") ?? throw new ArgumentNullException("DB_CONNECTION_STRING", "Connection string 'DB_CONNECTION_STRING' not found.");
// builder.Services.AddDbContext<AppDbContext>(options =>
// {
// 	options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
// });

builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization();

// builder.Services.AddIdentity<AppUser, AppRole>()
// 	.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<RoomService>();
// builder.Services.AddScoped<GeoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	// app.UseSwagger();
	// app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

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

app.MapControllers();

app.MapHub<GameHub>("/api/hub");
// app.MapCo

app.UsePathBase("/api");

app.Run();

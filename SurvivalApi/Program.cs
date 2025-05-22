using StackExchange.Redis;
using api.Hubs;
using api.Services;
using Quartz;
using Quartz.AspNetCore;
using Microsoft.EntityFrameworkCore;
using api.Data;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

var redisConnectionString = builder.Configuration["ConnectionStrings:REDIS_CONNECTION_STRING"] ?? throw new ArgumentNullException("REDIS_CONNECTION_STRING", "Connection string 'REDIS_CONNECTION_STRING' not found.");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

var connectionString = builder.Configuration["ConnectionStrings:DB_CONNECTION_STRING"] ?? throw new ArgumentNullException("DB_CONNECTION_STRING", "Connection string 'DB_CONNECTION_STRING' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseNpgsql(connectionString);
});

builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

// builder.Services.AddIdentity<AppUser, AppRole>()
// 	.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddQuartz(q =>
{
	// base Quartz scheduler, job and trigger configuration
});

// ASP.NET Core hosting
builder.Services.AddQuartzServer(options =>
{
	// when shutting down we want jobs to complete gracefully
	options.WaitForJobsToComplete = true;
});

builder.Services.AddScoped<RoomService>();
// builder.Services.AddScoped<GeoService>();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFrontend", policy =>
	{
		policy.WithOrigins("http://localhost:5173")
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
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

app.UseCors("AllowFrontend");

app.Run();

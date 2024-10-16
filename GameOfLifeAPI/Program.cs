using GameOfLifeAPI.Interfaces;
using GameOfLifeAPI.Services;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Game Of Life .NET API",
        Description = ".NET Core Web API for Game of Life Board State Management"
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Register health check
builder.Services.AddHealthChecks();

builder.Services.AddTransient<IGameOfLifeService, GameOfLifeService>();

// Add Redis configuration - resiliency policy defined in redisConfiguration
string? redisConfiguration = builder.Configuration.GetSection("Redis")["ConnectionString"];
if (string.IsNullOrEmpty(redisConfiguration))
{
    throw new InvalidOperationException("Redis connection string is not configured.");
}

ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisConfiguration);
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

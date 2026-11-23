using FluentValidation;
using StackExchange.Redis;
using TripPlanning.Api.BackgroundServices;
using TripPlanning.Api.ExceptionHandling;
using TripPlanning.Application.Common.Abstractions;
using TripPlanning.Application.Features.PlanTrip.Services;
using TripPlanning.Application.Features.PlanTrip.Validators;
using TripPlanning.Domain.Features.PlanTrip.Services;
using TripPlanning.Infrastructure.Caching;
using TripPlanning.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Problem Details and Global Exception Handler
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Redis Configuration
var redisOptions = builder.Configuration
    .GetSection(RedisOptions.SectionName)
    .Get<RedisOptions>()
    ?? throw new InvalidOperationException(
        "Redis configuration is missing.");

if (string.IsNullOrWhiteSpace(redisOptions.ConnectionString))
{
    throw new InvalidOperationException(
        "Redis connection string is missing.");
}

// Redis Connection
var redisConnection =
    ConnectionMultiplexer.Connect(
        redisOptions.ConnectionString);

builder.Services.AddSingleton<IConnectionMultiplexer>(
    redisConnection);

// Redis Snapshot Reader
builder.Services.AddSingleton<
    IMetroNetworkSnapshotReader,
    RedisMetroNetworkSnapshotReader>();

// Metro Network Graph
builder.Services.AddSingleton<MetroGraphBuilder>();

builder.Services.AddSingleton<MetroNetworkStateProvider>();

// Background Service: Sync Redis Snapshot with Local Graph
builder.Services.AddHostedService<MetroNetworkSyncService>();

// Plan Trip Services
builder.Services.AddScoped<MetroPathFinder>();
builder.Services.AddScoped<PlanTripService>();
builder.Services.AddScoped<TicketFareCalculator>();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<
    PlanTripRequestValidator>();

var app = builder.Build();

// Global Exception Handling
app.UseExceptionHandler();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
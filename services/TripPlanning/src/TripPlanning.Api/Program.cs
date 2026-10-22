
using FluentValidation;
using TripPlanning.Api.ExceptionHandling;
using TripPlanning.Application.Features.PlanTrip.Abstractions;
using TripPlanning.Application.Features.PlanTrip.Services;
using TripPlanning.Application.Features.PlanTrip.Validators;
using TripPlanning.Domain.Features.PlanTrip.Services;
using TripPlanning.Infrastructure.Clients;

var builder = WebApplication.CreateBuilder(args);

// Register Controllers
builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Problem Details and Global Exception Handler
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Configure MetroNetwork HTTP Client
var metroNetworkBaseUrl =
    builder.Configuration["Services:MetroNetwork:BaseUrl"]
    ?? throw new InvalidOperationException(
        "MetroNetwork BaseUrl is not configured.");

builder.Services.AddHttpClient<IMetroNetworkClient, MetroNetworkClient>(
    client =>
    {
        client.BaseAddress = new Uri(metroNetworkBaseUrl);
    });

// Register PlanTrip Services
builder.Services.AddScoped<MetroGraphBuilder>();
builder.Services.AddScoped<MetroPathFinder>();
builder.Services.AddScoped<PlanTripService>();
builder.Services.AddValidatorsFromAssemblyContaining<PlanTripRequestValidator>();

var app = builder.Build();

// Handle exceptions globally
app.UseExceptionHandler();

// Configure Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

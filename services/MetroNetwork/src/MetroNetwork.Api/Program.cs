using FluentValidation;
using FluentValidation.AspNetCore;
using MetroNetwork.Api.Common.ExceptionHandling;
using MetroNetwork.Application.Features.LineStations;
using MetroNetwork.Application.Features.MetroLines;
using MetroNetwork.Application.Features.MetroLines.Validators;
using MetroNetwork.Application.Features.Stations;
using MetroNetwork.Application.Features.Stations.Validators;
using MetroNetwork.Infrastructure.Features.LineStations;
using MetroNetwork.Infrastructure.Features.MetroLines;
using MetroNetwork.Infrastructure.Features.Stations;
using MetroNetwork.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<
    CreateStationRequestValidator>();

// Global Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Database
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MetroNetworkDbContext>(options =>
    options.UseSqlServer(connectionString));

// Dependency Injection
builder.Services.AddScoped<IStationRepository, StationRepository>();
builder.Services.AddScoped<IStationService, StationService>();

builder.Services.AddScoped<IMetroLineService, MetroLineService>();
builder.Services.AddScoped<IMetroLineRepository, MetroLineRepository>();

builder.Services.AddScoped<ILineStationRepository, LineStationRepository>();
builder.Services.AddScoped<ILineStationService, LineStationService>();

var app = builder.Build();

// Global Exception Handling Middleware
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

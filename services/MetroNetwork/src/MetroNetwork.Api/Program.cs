using FluentValidation;
using FluentValidation.AspNetCore;
using MetroNetwork.Api.Common.ExceptionHandling;
using MetroNetwork.Application.Features.Auth.Configuration;
using MetroNetwork.Application.Features.Auth.Repositories;
using MetroNetwork.Application.Features.Auth.Services;
using MetroNetwork.Application.Features.Auth.Validators;
using MetroNetwork.Application.Features.LineStations;
using MetroNetwork.Application.Features.MetroLines;
using MetroNetwork.Application.Features.Stations;
using MetroNetwork.Application.Features.Stations.Validators;
using MetroNetwork.Infrastructure.Features.LineStations;
using MetroNetwork.Infrastructure.Features.MetroLines;
using MetroNetwork.Infrastructure.Features.Stations;
using MetroNetwork.Infrastructure.Identity;
using MetroNetwork.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

builder.Services.AddValidatorsFromAssemblyContaining<
    LoginRequestValidator>();

// Global Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Database
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MetroNetworkDbContext>(options =>
    options.UseSqlServer(connectionString));

// JWT Configuration
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

var jwtOptions = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtOptions>()
    ?? throw new InvalidOperationException(
        "JWT configuration is missing.");

if (string.IsNullOrWhiteSpace(jwtOptions.SecretKey))
{
    throw new InvalidOperationException(
        "JWT SecretKey is missing.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Existing Features
builder.Services.AddScoped<IStationRepository, StationRepository>();
builder.Services.AddScoped<IStationService, StationService>();

builder.Services.AddScoped<IMetroLineService, MetroLineService>();
builder.Services.AddScoped<IMetroLineRepository, MetroLineRepository>();

builder.Services.AddScoped<ILineStationRepository, LineStationRepository>();
builder.Services.AddScoped<ILineStationService, LineStationService>();

// Auth
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

var app = builder.Build();

// Create initial admin if it does not exist
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<MetroNetworkDbContext>();

    await AdminSeeder.SeedAsync(
        app.Configuration,
        dbContext);
}

// Global Exception Handling Middleware
app.UseExceptionHandler();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
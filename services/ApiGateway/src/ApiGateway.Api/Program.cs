var builder = WebApplication.CreateBuilder(args);

// Register controllers
builder.Services.AddControllers();

// Configure YARP Reverse Proxy
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Register OpenAPI services
builder.Services.AddOpenApi();

var app = builder.Build();

// Enable OpenAPI endpoint in Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Enable authorization middleware
app.UseAuthorization();

// Map controller endpoints
app.MapControllers();

// Map YARP Reverse Proxy endpoints
app.MapReverseProxy();

app.Run();
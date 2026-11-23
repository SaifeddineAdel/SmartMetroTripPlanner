var builder = WebApplication.CreateBuilder(args);

// Register controllers
builder.Services.AddControllers();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AngularClient");

// Enable authorization middleware
app.UseAuthorization();

// Map controller endpoints
app.MapControllers();

// Map YARP Reverse Proxy endpoints
app.MapReverseProxy();

app.Run();
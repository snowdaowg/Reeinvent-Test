using SynonymSearchApp.Repositories;
using Microsoft.Extensions.Options;
using ReeinventTest2.Models;

var builder = WebApplication.CreateBuilder(args);

// Set the application to run on a specific port (5001 for HTTPS)
builder.WebHost.UseUrls("https://localhost:5001");

// Add services to the container
builder.Services.AddControllers();

// Configure CORS
builder.Services.Configure<CorsSettings>(builder.Configuration.GetSection("CorsSettings"));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>();

        if (corsSettings != null && corsSettings.AllowedOrigins.Any())
        {
            policy.WithOrigins(corsSettings.AllowedOrigins.ToArray())
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
        else
        {
            policy.WithOrigins("http://localhost:3000") // Local dev front-end URL
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
    });
});

// Register SynonymRepository for dependency injection
builder.Services.AddSingleton<ISynonymRepository, SynonymRepository>();

// Add Swagger for API documentation (useful for development)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable CORS middleware
app.UseCors();

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Serve static files and React fallback in production
if (!app.Environment.IsDevelopment())
{
    app.UseStaticFiles(); // Serve React build files
    app.MapFallbackToFile("index.html"); // Single fallback route for SPA
}

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Map API controllers
app.MapControllers();

app.Run();

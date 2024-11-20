using SynonymSearchApp.Repositories;
using Microsoft.Extensions.Options;
using ReeinventTest2.Models;

var builder = WebApplication.CreateBuilder(args);

// Set the application to run on a specific port (5001 for HTTPS)
builder.WebHost.UseUrls("https://localhost:5001"); // This will bind to port 5001 for HTTPS

// Add services to the container
builder.Services.AddControllers();

// Read CORS settings from appsettings.json (if you have custom origins defined in appsettings.json)
builder.Services.Configure<CorsSettings>(builder.Configuration.GetSection("CorsSettings"));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        // Retrieve the allowed origins from appsettings or set default origins
        var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>();

        if (corsSettings != null && corsSettings.AllowedOrigins.Any())
        {
            // Use the allowed origins from your appsettings
            foreach (var origin in corsSettings.AllowedOrigins)
            {
                policy.WithOrigins(origin)  // Allow the origin(s)
                      .AllowAnyHeader()  // Allow all headers
                      .AllowAnyMethod()  // Allow all HTTP methods (GET, POST, etc.)
                      .AllowCredentials(); // Allow credentials (if needed for cookies or authorization headers)
            }
        }
        else
        {
            // Default allowed origin for development (adjust as necessary)
            policy.WithOrigins("http://localhost:3000")  // Local dev front-end URL
                  .AllowAnyHeader()  // Allow all headers
                  .AllowAnyMethod()  // Allow all HTTP methods
                  .AllowCredentials(); // Allow credentials (optional, depending on your app)
        }
    });
});

// Register the SynonymRepository for dependency injection (make sure it's available in the app)
builder.Services.AddSingleton<ISynonymRepository, SynonymRepository>();

// Add Swagger services for API documentation (useful in development)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use CORS middleware (apply the CORS policy globally)
app.UseCors();

// Serve React app's static files from the 'build' directory in production
if (!app.Environment.IsDevelopment())
{
    app.UseStaticFiles(); // Serve static files like index.html, js, css, etc.
    app.MapFallbackToFile("index.html"); // Ensure React routing works with fallback to index.html
}

// Swagger UI in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable HTTPS redirection to ensure the app uses HTTPS in all environments
app.UseHttpsRedirection();

// Optional: Authorization middleware (if your app requires authentication)
app.UseAuthorization();

// Map controllers for API endpoints (ensure all controllers are routed correctly)
app.MapControllers();

app.Run();

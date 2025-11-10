using Microsoft.EntityFrameworkCore;
using PortfolioBackend.Config;
using PortfolioBackend.Utils;
using PortfolioBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// Only load user secrets in Development (they are not used in production)
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options => 
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }
    
    // In production we avoid writing to Console. Use ILogger for structured logging if needed.
    
    options.UseSqlServer(connectionString, sqlOptions => 
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});
builder.Services.AddDataProtection();
builder.Services.AddScoped<MailServiceHelper>();

builder.Services.AddOptions<AppSettings>()
    .Bind(builder.Configuration.GetSection("AppSettings"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

string? frontendOrigin = builder.Configuration["FRONTEND_ORIGIN"];
if (string.IsNullOrWhiteSpace(frontendOrigin))
    throw new InvalidOperationException("FRONTEND_ORIGIN is not set.");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClientSide", policy =>
    {
        policy.WithOrigins(frontendOrigin)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowClientSide");
app.MapControllers();

app.Run();

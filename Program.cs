using Microsoft.EntityFrameworkCore;
using PortfolioBackend.Contexts;
using PortfolioBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Env & Secret Configuration.
builder.Configuration
    .AddEnvironmentVariables()  // Works for GitHub Action Secrets
    .AddUserSecrets<Program>(); // Works for local development
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Service & Controller Configuration.
builder.Services.AddControllers(); // Add serves to container.
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers(); // Map controllers to routes.

app.Run();

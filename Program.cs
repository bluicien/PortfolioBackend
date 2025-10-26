using Microsoft.EntityFrameworkCore;
using PortfolioBackend.Config;
using PortfolioBackend.Utils;
using PortoflioBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add DbContext using SQL Server Provider
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));
builder.Configuration.AddUserSecrets<Program>();
builder.Services.AddDataProtection();
builder.Services.AddScoped<MailServiceHelper>();

builder.Services.AddOptions<AppSettings>()
    .Bind(builder.Configuration.GetSection("AppSettings"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();
app.MapControllers();

app.Run();

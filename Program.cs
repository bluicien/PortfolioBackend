using PortfolioBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Read from environment
var allowedOriginsRaw = builder.Configuration["Cors:AllowedOrigins"];
var allowedOrigins = allowedOriginsRaw?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy("DynamicCorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});



// 🌱 Environment & Secret Configuration
builder.Configuration
    .AddEnvironmentVariables()  // For GitHub Action Secrets
    .AddUserSecrets<Program>(); // For local development

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// 🧠 Cosmos DB Service Registration
builder.Services.AddSingleton<CosmosDbService>();

// 🛠️ Service & Controller Configuration
builder.Services.AddControllers();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();

// 📘 OpenAPI / Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors("DynamicCorsPolicy");

// 🚀 Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
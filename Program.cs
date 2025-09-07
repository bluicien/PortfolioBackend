using PortfolioBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// 🌱 Environment & Secret Configuration
builder.Configuration
    .AddEnvironmentVariables()  // For GitHub Action Secrets
    .AddUserSecrets<Program>(); // For local development

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// 🧠 Cosmos DB Service Registration
builder.Services.AddSingleton<CosmosDbService>();

// 🧹 Remove EF Core setup
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🛠️ Service & Controller Configuration
builder.Services.AddControllers();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();

// 📘 OpenAPI / Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

// 🚀 Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
using Microsoft.Azure.Cosmos;
using Azure.Identity;

namespace PortfolioBackend.Services
{
public class CosmosDbService
    {
        private readonly IConfiguration _config;
        private CosmosClient? _client;
        private Database? _database;
        private readonly Dictionary<string, Container> _containerCache = new();

        // Optional: map container names to partition key paths
        private readonly Dictionary<string, string> _partitionKeyMap = new()
        {
            { "Feedbacks", "/username" },
            { "Conversations", "/userId" },
            { "Products", "/productType" },
            { "Users", "/userId" },
            { "Messages", "/userId" }
        };

        public CosmosDbService(IConfiguration config)
        {
            _config = config;
        }

        public async Task InitializeAsync()
        {
            try
            {
                Console.WriteLine("Starting Cosmos DB initialization...");
                Console.WriteLine($"END POINT: {_config["COSMOS_DB_ENDPOINT"]}");
                Console.WriteLine($"DB NAME: {_config["COSMOS_DB_DATABASE_NAME"]}");

                var endpoint = _config["COSMOS_DB_ENDPOINT"] ?? throw new InvalidOperationException("Missing endpoint");
                var dbName = _config["COSMOS_DB_DATABASE_NAME"] ?? throw new InvalidOperationException("Missing database name");

                _client = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                    ? new CosmosClient(endpoint, _config["CosmosDb:Key"])
                    : new CosmosClient(endpoint, new DefaultAzureCredential());

                var response = await _client.CreateDatabaseIfNotExistsAsync(dbName);
                _database = response.Database;

                foreach (var kvp in _partitionKeyMap)
                {
                    var containerResponse = await _database.CreateContainerIfNotExistsAsync(kvp.Key, kvp.Value);
                    _containerCache[kvp.Key] = containerResponse.Container;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cosmos DB initialization failed: {ex.Message}");
                throw;
            }
        }

        public Container GetContainer(string containerName)
        {
            if (_containerCache.TryGetValue(containerName, out var container))
                return container;

            throw new InvalidOperationException($"Container '{containerName}' not initialized.");
        }
    }
}
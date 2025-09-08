using Microsoft.Azure.Cosmos;

namespace PortfolioBackend.Services
{
public class CosmosDbService
    {
        private readonly CosmosClient _client;
        private readonly Database _database;
        private readonly Dictionary<string, Container> _containerCache = new();

        // Optional: map container names to partition key paths
        private readonly Dictionary<string, string> _partitionKeyMap = new()
        {
            { "Feedbacks", "/userId" },
            { "Conversations", "/sessionId" },
            { "Products", "/productType" },
            { "Users", "/userId" },
            { "Messages", "/conversationId" }
        };

        public CosmosDbService(IConfiguration config)
        {
            var endpoint = config["CosmosDb:Endpoint"];
            var key = config["CosmosDb:Key"];
            var dbName = config["CosmosDb:DatabaseName"];

            _client = new CosmosClient(endpoint, key);
            _database = _client.CreateDatabaseIfNotExistsAsync(dbName).Result.Database;
        }

        public Container GetContainer(string containerName)
        {
            if (_containerCache.ContainsKey(containerName))
                return _containerCache[containerName];

            var partitionKeyPath = _partitionKeyMap.ContainsKey(containerName)
                ? _partitionKeyMap[containerName]
                : "/partitionKey"; // fallback default

            var container = _database
                .CreateContainerIfNotExistsAsync(containerName, partitionKeyPath)
                .Result
                .Container;

            _containerCache[containerName] = container;
            return container;
        }
    }
}
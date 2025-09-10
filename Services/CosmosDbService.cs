using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.CosmosDB;
using Azure.ResourceManager.CosmosDB.Models;
using Microsoft.Azure.Cosmos;

namespace PortfolioBackend.Services
{
    public class CosmosDbService
    {
        private readonly IConfiguration _config;
        private CosmosClient? _client;
        private Database? _database;
        private readonly Dictionary<string, Container> _containerCache = new();

        // map container names to partition key paths
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
            // 1. Provision via ARM SDK
            var subscriptionId = _config["AZURE_SUBSCRIPTION_ID"]
                ?? throw new InvalidOperationException("Missing subscription ID");
            var rgName = _config["COSMOS_DB_RESOURCE_GROUP"]
                ?? throw new InvalidOperationException("Missing resource group");
            var accountName = _config["COSMOS_DB_ACCOUNT_NAME"]
                ?? throw new InvalidOperationException("Missing account name");
            var databaseName = _config["COSMOS_DB_DATABASE_NAME"]
                ?? throw new InvalidOperationException("Missing database name");

            Console.WriteLine("Starting ARM provisioning...");
            var credential = new DefaultAzureCredential();
            var armClient = new ArmClient(credential);

            // Build the resource identifier for your Cosmos DB account
            var accountId = CosmosDBAccountResource
                .CreateResourceIdentifier(subscriptionId, rgName, accountName);

            // Fetch the Cosmos DB account resource
            var accountResource = await armClient
                .GetCosmosDBAccountResource(accountId)
                .GetAsync();

            // Create or update the SQL database
            var dbCollection = accountResource.Value.GetCosmosDBSqlDatabases();
            var dbData = new CosmosDBSqlDatabaseResourceInfo(databaseName);
            var dbContent = new CosmosDBSqlDatabaseCreateOrUpdateContent(
                accountResource.Value.Data.Location,
                dbData);
            var dbResponse = await dbCollection
                .CreateOrUpdateAsync(WaitUntil.Completed, databaseName, dbContent);

            // Create or update each container
            var sqlDbResource = dbResponse.Value;
            var containerCollection = sqlDbResource.GetCosmosDBSqlContainers();

            foreach (var kvp in _partitionKeyMap)
            {
                var containerId = kvp.Key;
                var pk = new CosmosDBContainerPartitionKey
                {
                    Kind = CosmosDBPartitionKind.Hash,
                    Paths = { kvp.Value }
                };
                var containerData = new CosmosDBSqlContainerResourceInfo(containerId)
                {
                    PartitionKey = pk
                };

                var containerContent = new CosmosDBSqlContainerCreateOrUpdateContent(
                    accountResource.Value.Data.Location,
                    containerData
                );
                
                await containerCollection
                    .CreateOrUpdateAsync(WaitUntil.Completed, containerId, containerContent);

                Console.WriteLine($"Provisioned container: {containerId}");
            }

            Console.WriteLine("ARM provisioning complete.");

            // 2. Initialize data-plane client for CRUD
            Console.WriteLine("Starting data-plane client...");
            var endpoint = _config["COSMOS_DB_ENDPOINT"]!;
            _client = new CosmosClient(endpoint, credential);

            // Cache Database and Containers for data operations
            _database = _client.GetDatabase(databaseName);
            foreach (var kvp in _partitionKeyMap)
            {
                _containerCache[kvp.Key] = _database.GetContainer(kvp.Key);
            }

            Console.WriteLine("Data-plane client initialization complete.");
        }

        public Container GetContainer(string containerName)
        {
            if (_containerCache.TryGetValue(containerName, out var container))
                return container;

            throw new InvalidOperationException($"Container '{containerName}' not initialized.");
        }
    }
}
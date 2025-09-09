using Microsoft.Azure.Cosmos;
using PortfolioBackend.Models;

namespace PortfolioBackend.Services
{
    public class ConversationService : IConversationService
    {
        private readonly Container _container;

        public ConversationService(CosmosDbService cosmosDbService)
        {
            _container = cosmosDbService.GetContainer("Conversations");
        }

        public async Task<IEnumerable<Messages>> GetMessages(string conversationId, string userId)
        {
            try
            {
                var query = _container.GetItemQueryIterator<Messages>(
                    new QueryDefinition("SELECT * FROM c WHERE c.conversationId = @conversationId")
                    .WithParameter("@conversationId", conversationId),
                    requestOptions: new QueryRequestOptions
                    {
                        PartitionKey = new PartitionKey(userId)
                    }
                );

                var results = new List<Messages>();
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    results.AddRange(response);
                }

                return results;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return [];
            }
        }

        public async Task SendMessage(Messages message)
        {
            message.Id ??= Guid.NewGuid().ToString();
            message.CreatedAt = DateTime.UtcNow;

            await _container.CreateItemAsync(message, new PartitionKey(message.UserId));
        }
    }
}
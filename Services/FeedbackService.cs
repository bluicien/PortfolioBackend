using Microsoft.Azure.Cosmos;
using PortfolioBackend.Models;

namespace PortfolioBackend.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly Container _container;

        public FeedbackService(CosmosDbService cosmosDbService)
        {
            _container = cosmosDbService.GetContainer("Feedbacks");
        }

        public async Task<Feedback?> GetFeedbackByIdAsync(string id, string partitionKey)
        {
            try
            {
                var response = await _container.ReadItemAsync<Feedback>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksAsync()
        {
            var query = _container.GetItemQueryIterator<Feedback>(
                new QueryDefinition("SELECT * FROM c"));

            var results = new List<Feedback>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task SendFeedbackAsync(Feedback feedback)
        {
            feedback.Id ??= Guid.NewGuid().ToString();
            feedback.CreatedAt = DateTime.UtcNow;

            await _container.CreateItemAsync(feedback, new PartitionKey(feedback.Username));
        }
    }
    
}
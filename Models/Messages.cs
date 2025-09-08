using Newtonsoft.Json;

namespace PortfolioBackend.Models
{
    public class Messages
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("userId")]
        public required string UserId { get; set; }

        [JsonProperty("conversationId")]
        public required string ConversationId { get; set; } // Partition Key

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
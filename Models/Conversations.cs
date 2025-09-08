using Newtonsoft.Json;

namespace PortfolioBackend.Models
{
    public class Conversations
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("userId")]
        public required string UserId { get; set; } // Partition Key

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
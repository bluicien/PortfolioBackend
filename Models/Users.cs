using Newtonsoft.Json;

namespace PortfolioBackend.Models
{
    public class Users
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("userId")]
        public required string UserId { get; set; } // Partition Key
        
        [JsonProperty("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty("username")]
        public required string Username { get; set; }

        [JsonProperty("password")]
        public required string Password { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
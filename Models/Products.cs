using Newtonsoft.Json;

namespace PortfolioBackend.Models
{
    public class Products
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("productName")]
        public string ProductName { get; set; } = string.Empty;

        [JsonProperty("productType")]
        public string ProductType { get; set; } = String.Empty; // Partition Key

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
using Newtonsoft.Json;

namespace PortfolioBackend.Models
{
    public class Feedback
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("username")]
        public string Username { get; set; } = "Anonymous"; // Partition Key

        [JsonProperty("companyRole")]
        public string? CompanyRole { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonProperty("isApproved")]
        public bool IsApproved { get; set; } = false;
    }
}
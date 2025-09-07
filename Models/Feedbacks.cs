using Newtonsoft.Json;

namespace PortfolioBackend.Models
{
public class Feedback
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("userId")]
    public string UserId { get; set; } = "default";

    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;

    [JsonProperty("companyRole")]
    public string? CompanyRole { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;

    [JsonProperty("submittedAt")]
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    [JsonProperty("isApproved")]
    public bool IsApproved { get; set; } = false;
}
}
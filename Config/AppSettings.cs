using System.ComponentModel.DataAnnotations;

namespace PortfolioBackend.Config;

public class AppSettings
{
    [Required]
    public string ServerUrl { get; set; } = string.Empty;
    [Required]
    public string DomainAddress { get; set; } = string.Empty;
    [Required]
    public string EmailServiceApiKey { get; set; } = string.Empty;
    [Required]
    public string MyEmailAddress { get; set; } = string.Empty;
}

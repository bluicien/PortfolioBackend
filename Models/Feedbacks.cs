using System.ComponentModel.DataAnnotations;

namespace PortfolioBackend.Models
{
    public class Feedbacks
    {
        [Key]
        public int FeedbackId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? CompanyRole { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
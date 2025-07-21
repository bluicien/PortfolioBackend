using System.ComponentModel.DataAnnotations;

namespace PortfolioBackend.Models
{
    public class Conversations
    {
        [Key]
        public int ConversationId { get; set; }
        public required ICollection<Messages> Messages { get; set; }
        public int? UserId { get; set; }
        public Users? User { get; set; }
    }
}
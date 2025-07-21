using System.ComponentModel.DataAnnotations;

namespace PortfolioBackend.Models
{
    public class Messages
    {
        [Key]
        public int MessageId { get; set; }
        public int ConversationId { get; set; }
        public required Conversations Conversation { get; set; }
    }
}
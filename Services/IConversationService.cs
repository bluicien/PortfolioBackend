using PortfolioBackend.Models;

namespace PortfolioBackend.Services
{
    public interface IConversationService
    {
        Task<IEnumerable<Messages>> GetMessages(string conversationId, string userId);
        Task SendMessage(Messages message);
    }
}
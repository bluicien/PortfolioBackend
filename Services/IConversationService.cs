using PortfolioBackend.Models;

namespace PortfolioBackend.Services
{
    public interface IConversationService
    {
        Task<string> GetGeneratedTextAsync(string prompt);
        IEnumerable<Messages> GetMessages(int conversationId);
        Conversations SendMessage(Messages message);
    }
}
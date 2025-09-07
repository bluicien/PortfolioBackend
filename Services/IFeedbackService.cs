using PortfolioBackend.Models;

namespace PortfolioBackend.Services
{
    public interface IFeedbackService
    {
        Task<IEnumerable<Feedback>> GetFeedbacksAsync();
        Task<Feedback?> GetFeedbackByIdAsync(string id, string partitionKey);
        Task SendFeedbackAsync(Feedback feedback);
    }
}
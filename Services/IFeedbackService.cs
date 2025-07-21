using PortfolioBackend.Models;

namespace PortfolioBackend.Services
{
    public interface IFeedbackService
    {
        IEnumerable<Feedbacks>? GetFeedbacks();
        void SendFeedback(Feedbacks feedbacks);
    }
}
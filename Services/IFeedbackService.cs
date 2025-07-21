using PortfolioBackend.Models;

namespace PortfolioBackend.Services
{
    public interface IFeedbackService
    {
        IEnumerable<Feedbacks>? GetFeedbacks();
        Feedbacks? GetFeedbackById(int id);
        void SendFeedback(Feedbacks feedbacks);
    }
}
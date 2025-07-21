using PortfolioBackend.Models;

namespace PortfolioBackend.Services
{
    public interface IFeedbackService
    {
        IEnumerable<Feedback>? GetFeedback();
        void SendFeedBack(Feedback feedback);
    }
}
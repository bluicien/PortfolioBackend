using PortfolioBackend.Models;
using PortfolioBackend.Contexts;

namespace PortfolioBackend.Services
{
    public class FeedbackService(ApplicationDbContext context) : IFeedbackService
    {
        private readonly ApplicationDbContext _context = context;

        public Feedbacks? GetFeedbackById(int id)
        {
            return _context.Feedbacks.FirstOrDefault(feedback => feedback.FeedbackId == id);
        }

        public IEnumerable<Feedbacks>? GetFeedbacks()
        {
            return _context.Feedbacks.ToList();
        }

        public void SendFeedback(Feedbacks feedback)
        {
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();
        }
    }
}
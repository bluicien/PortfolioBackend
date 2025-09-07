using Microsoft.AspNetCore.Mvc;
using PortfolioBackend.Models;
using PortfolioBackend.Services;

namespace PortfolioBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // GET: api/feedback
        [HttpGet]
        public async Task<IActionResult> GetFeedbacks()
        {
            var feedbacks = await _feedbackService.GetFeedbacksAsync();
            if (!feedbacks.Any())
                return NotFound("No feedback found.");

            return Ok(feedbacks);
        }

        // GET: api/feedback/{id}?partitionKey=xyz
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeedbackById(string id, [FromQuery] string partitionKey = "default")
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(id, partitionKey);
            if (feedback == null)
                return NotFound($"No feedback with ID '{id}' found.");

            return Ok(feedback);
        }

        // POST: api/feedback
        [HttpPost]
        public async Task<IActionResult> SendFeedback([FromBody] Feedback feedback)
        {
            if (feedback == null)
                return BadRequest("Request must contain a feedback object.");

            feedback.UserId = feedback.Username ?? "default";
            await _feedbackService.SendFeedbackAsync(feedback);

            return CreatedAtAction(nameof(GetFeedbackById), new { id = feedback.Id, partitionKey = feedback.UserId }, feedback);
        }
    }
}
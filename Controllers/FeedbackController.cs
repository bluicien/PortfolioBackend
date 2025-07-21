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

        [HttpGet]
        public IActionResult GetFeedbacks()
        {
            Feedbacks[] feedbacks = _feedbackService.GetFeedbacks()?.ToArray() ?? [];
            if (feedbacks.Length == 0) return NotFound("No feedback found.");
            return Ok(feedbacks);
        }

        [HttpGet("{id}")]
        public IActionResult GetFeedbackById(int id)
        {
            Feedbacks? feedback = _feedbackService.GetFeedbackById(id);
            if (feedback == null) return NotFound($"No feedback of ID {id} found.");

            return Ok(feedback);
        }

        [HttpPost]
        public IActionResult SendFeedback([FromBody] Feedbacks feedback)
        {
            if (feedback == null) return BadRequest("Request must contain feedback object.");
            _feedbackService.SendFeedback(feedback);
            return StatusCode(201, feedback);
        }
    }
}
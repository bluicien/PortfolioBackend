using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioBackend.Utils;
using PortoflioBackend.Data;
using PortoflioBackend.Models;

namespace PortoflioBackend.Controller;

[Route("api/[controller]")]
[ApiController]
public class FeedbackController : ControllerBase
{
    private readonly ILogger<FeedbackController> _logger;
    private readonly AppDbContext _dbContext;
    private readonly MailServiceHelper _mailHelper;

    public FeedbackController(ILogger<FeedbackController> logger, AppDbContext dbContext, MailServiceHelper mailHelper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mailHelper = mailHelper;
    }


    [HttpGet]
    public async Task<ActionResult<List<Feedback>>> GetFeedbacks()
    {
        try
        {
            var dbFeedback = await _dbContext.Feedback.ToListAsync();

            if (dbFeedback != null && dbFeedback.Count > 0)
            {
                _logger.LogInformation("Returning {Count} feedback entries.", dbFeedback.Count);
                return dbFeedback;
            }

            _logger.LogInformation("No feedback found.");
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving feedback list.");
            return Problem(
                detail: ex.Message,
                title: "An internal server error has occurred"
            );
        }
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Feedback>> GetFeedbackById(int id)
    {
        try
        {
            Feedback? feedback = await _dbContext.Feedback.FirstOrDefaultAsync(f => f.FeedbackId == id);

            if (feedback != null)
            {
                return feedback;
            }

            _logger.LogInformation("No feedback found with ID: {Id}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving feedback with ID {Id}.", id);
            return Problem(
                detail: ex.Message,
                title: "An internal server error has occurred"
            );
        }
    }


    [HttpGet("approve")]
    public async Task<IActionResult> ApproveFeedback([FromQuery] string token)
    {
        int feedbackId;
        try
        {
            string decrypted = _mailHelper.DecryptLink(token);
            if (!int.TryParse(decrypted, out feedbackId))
            {
                return Content("<h2>❌ Invalid Token</h2><p>This link is invalid or has expired.</p>", "text/html");
            }
        }
        catch
        {
            _logger.LogWarning("Attempted approval with invalid token: {Token}", token);
            return Content("<h2>❌ Invalid Token</h2><p>This link is invalid or has expired.</p>", "text/html");
        }

        try
        {
            Feedback? feedback = await _dbContext.Feedback.FindAsync(feedbackId);
            if (feedback == null)
                return Content("<h2>❌ 404 Not Found</h2><p>No feedback entry exists with this ID</p>", "text/html");

            if (feedback.IsApproved == true)
            {
                _logger.LogInformation($"Feedback #{feedbackId} already approved.");
                return Content("<h2>❌Approval Failed</h2><p>This feedback has already been approved.</p>", "text/html");
            }

            feedback.IsApproved = true;
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Feedback #{feedbackId} approved successfully.");
            return Content("<h2>✅Feedback Approved</h2><p>Thanks for reviewing!</p>", "text/html");
        }
        catch (Exception ex)
        {
            return Problem(
                detail: ex.Message,
                title: "An internal server error has occurred"
            );
        }
    }


    [HttpPost]
    public async Task<ActionResult<Feedback>> Create([FromBody] Feedback newFeedback)
    {
        try
        {
            await _dbContext.AddAsync(newFeedback);
            int rowsAffected = await _dbContext.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                _logger.LogInformation("Feedback saved (ID: {Id}).", newFeedback.FeedbackId);
                await _mailHelper.SendApprovalEmailAsync(newFeedback);

                return CreatedAtAction(nameof(GetFeedbackById), new { id = newFeedback.FeedbackId }, newFeedback);
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation("Bad request when creating feedback.");
                    return BadRequest(ModelState);
                }
                else
                {
                    string errorMessage = "Failed to add feedback";
                    _logger.LogError(errorMessage);
                    throw new Exception(errorMessage);
                }
            }
        }
        catch (Exception ex)
        {
            return Problem(
                detail: ex.Message,
                title: "An internal server error has occurred"
            );
        }
    }
}
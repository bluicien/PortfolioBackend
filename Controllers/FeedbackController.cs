using Microsoft.AspNetCore.Mvc;
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
    public ActionResult<List<Feedback>> GetFeedbacks()
    {
        List<Feedback> dbFeedback = [.. _dbContext.Feedback];

        try
        {
            if (dbFeedback != null && dbFeedback.Count > 0)
            {
                Console.WriteLine("Returning feedback...");
                return dbFeedback;
            }
            else
            {
                Console.WriteLine("No feedback found.");
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
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
            Feedback? feedback = _dbContext.Feedback.Where(f => f.UserId == id).FirstOrDefault();

            if (feedback != null)
            {
                return feedback;
            }
            else
            {
                Console.WriteLine($"No feedback found with ID: {id}");
                return NotFound();
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


    [HttpPost]
    public async Task<ActionResult<Feedback>> Create([FromBody] Feedback newFeedback)
    {
        try
        {
            await _dbContext.AddAsync(newFeedback);
            int rowsAffected = await _dbContext.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Save successful!");
                await _mailHelper.SendApprovalEmailAsync(newFeedback);

                return CreatedAtAction(nameof(GetFeedbackById), new { id = newFeedback.UserId }, newFeedback);
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("Bad Request, failed to create");
                    return BadRequest(ModelState);
                }
                else
                {
                    string errorMessage = "Failed to add feedback";
                    Console.WriteLine(errorMessage);
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
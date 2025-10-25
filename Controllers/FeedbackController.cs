using Microsoft.AspNetCore.Mvc;
using PortoflioBackend.Data;
using PortoflioBackend.Models;

namespace PortoflioBackend.Controller;

[Route("api/[controller]")]
[ApiController]
public class FeedbackController : ControllerBase
{
    private readonly ILogger<FeedbackController> _logger;
    private readonly AppDbContext _dbContext;

    public FeedbackController(ILogger<FeedbackController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet]
    public IEnumerable<Feedback> Feedback()
    {
        var dbFeedback = _dbContext.Feedback.ToList();
        if (dbFeedback != null && dbFeedback.Count > 0)
        {
            return dbFeedback;
        }

        throw new Exception("Unable to fetch data");
    }
}
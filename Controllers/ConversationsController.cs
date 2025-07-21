using Microsoft.AspNetCore.Mvc;
using PortfolioBackend.Services;


namespace PortfolioBackend.Controllers
{
    [ApiController]
    [Route("/api/conversations")]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationService _conversationService;

        public ConversationsController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> GetResponseMessage([FromBody] string prompt)
        {
            var result = await _conversationService.GetGeneratedTextAsync(prompt);
            return Ok(result);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using PortfolioBackend.Models;
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
        public async Task<IActionResult> GetResponseMessage([FromBody] Messages message)
        {
            try
            {
                await _conversationService.SendMessage(message);
                return Ok();
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
    }
}
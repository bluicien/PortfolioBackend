using Mscc.GenerativeAI;
using PortfolioBackend.Models;

namespace PortfolioBackend.Services
{
    public class ConversationService : IConversationService
    {
        private readonly GenerativeModel _gemini;

        public ConversationService(IConfiguration configuration)
        {
            string apiKey;
            apiKey = configuration["GoogleGeminiApiKey"]
                ?? throw new InvalidOperationException("API key is missing!");

            GoogleAI googleAI = new(apiKey);
            _gemini = googleAI.GenerativeModel(model: Model.Gemini20Flash);
        }

        public async Task<string> GetGeneratedTextAsync(string prompt)
        {
            var response = await _gemini.GenerateContent(prompt);
            return response.Text ?? "No Response";
        }

        public IEnumerable<Messages> GetMessages(int conversationId)
        {
            throw new NotImplementedException();
        }

        public Conversations SendMessage(Messages message)
        {
            throw new NotImplementedException();
        }
    }
}
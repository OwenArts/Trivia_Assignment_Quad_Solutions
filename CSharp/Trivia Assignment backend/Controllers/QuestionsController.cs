using Microsoft.AspNetCore.Mvc;
using Trivia_Assignment_backend.Managers;

namespace Trivia_Assignment_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionsController : ControllerBase
    {
        private int MaxAmount = 100;
        private int MinAmount = 1;

        private readonly ILogger<QuestionsController> _logger;
        private readonly QuestionSessionManager _questionSessionManager;

        public QuestionsController(ILogger<QuestionsController> logger, QuestionSessionManager questionSessionManager)
        {
            _logger = logger;
            _questionSessionManager = questionSessionManager;
        }

        [HttpGet]
        public async Task<ActionResult<String>> GetAsync(int Amount)
        {
            if (Amount < MinAmount || Amount > MaxAmount)
                return BadRequest($"Requested amount must be between {MinAmount} an {MaxAmount}.");

            var questions = await _questionSessionManager.FetchQuestionsAsync(Amount);

            if (questions == null || !questions.Any())
                return BadRequest("Could not retrieve questions.");

            return Ok(questions);
        }
    }
}

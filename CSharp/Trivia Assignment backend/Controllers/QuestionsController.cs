using Microsoft.AspNetCore.Mvc;

namespace Trivia_Assignment_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionsController : ControllerBase
    {
        private int MaxAmount = 100;
        private int MinAmount = 1;

        private readonly ILogger<QuestionsController> _logger;

        public QuestionsController(ILogger<QuestionsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<String>> Get(int Amount)
        {
            if (Amount < MinAmount || Amount > MaxAmount)
            {
                return BadRequest($"Requested amount must be between {MinAmount} an {MaxAmount}.");
            }

            return Enumerable.Range(1, 5).Select(index => $"{Amount}").ToArray();
        }
    }
}

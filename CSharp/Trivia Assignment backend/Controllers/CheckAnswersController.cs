using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using Trivia_Assignment_backend.Managers;
using Trivia_Assignment_backend.Models;

namespace Trivia_Assignment_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckAnswersController : Controller
    {
        private readonly ILogger<CheckAnswersController> _logger;
        private readonly QuestionSessionManager _questionSessionManager;

        public CheckAnswersController(ILogger<CheckAnswersController> logger, QuestionSessionManager questionSessionManager)
        {
            _logger = logger;
            _questionSessionManager = questionSessionManager;
        }

        [HttpPost]
        public ActionResult<String> Post([FromBody] List<AnswerModel> questionAnswers)
        {
            if (questionAnswers == null || !questionAnswers.Any())
                return BadRequest("questionAnswers are required in the body.");

            try
            {
                foreach (var answer in questionAnswers)
                {
                    _questionSessionManager.CheckSingleAnswer(answer);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(questionAnswers);
        }
    }
}

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

        [HttpPost("{QuestionId}")]
        public ActionResult<String> Post(String QuestionId, string QuestionAnswer)
        {
            if (string.IsNullOrEmpty(QuestionAnswer) || string.IsNullOrEmpty(QuestionAnswer))
            {
                return BadRequest("QuestionId and QuestionAnswer is required.");
            }

            Guid id;
            try
            {
                Guid.TryParse(QuestionId, out id);
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to parse the given Question Id.");
            }

            AnswerModel answer = new AnswerModel(id, QuestionAnswer);

            try
            {
                _questionSessionManager.CheckSingleAnswers(answer);
            }
            catch (ArgumentException ex)
            {
                BadRequest("Unable to find the given Question Id.");
            }

            return JsonConvert.SerializeObject(answer);
        }

        [HttpPost]
        public ActionResult<String> Post([FromBody] List<AnswerModel> QuestionAnswers)
        {
            if (QuestionAnswers.Count <= 0)
                return BadRequest("QuestionAnswers are required in the body.");

            foreach (var answer in QuestionAnswers)
            {
                _questionSessionManager.CheckSingleAnswers(answer);
            }

            return JsonConvert.SerializeObject(QuestionAnswers);
        }
    }
}

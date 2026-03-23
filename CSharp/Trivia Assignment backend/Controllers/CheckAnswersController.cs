using Microsoft.AspNetCore.Mvc;

namespace Trivia_Assignment_backend.Controllers
{
    public class CheckAnswersController : Controller
    {
        private readonly ILogger<CheckAnswersController> _logger;

        public CheckAnswersController(ILogger<CheckAnswersController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult Post(String QuestionSetId)
        {
            if (string.IsNullOrEmpty(QuestionSetId))
            {
                return BadRequest("QuestionSetId is required.");
            }

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

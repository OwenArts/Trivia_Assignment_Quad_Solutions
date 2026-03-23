using Trivia_Assignment_backend.Models;
using Trivia_Assignment_backend.Models.Reponses;

namespace Trivia_Assignment_backend.Managers
{
    public class QuestionSessionManager
    {
        private readonly ILogger<QuestionSessionManager> _logger;
        private readonly Dictionary<string, List<QuestionModel>> _sessions = new Dictionary<string, List<QuestionModel>>();
        private readonly string ApiUrl = "https://opentdb.com/api.php";

        public QuestionSessionManager(ILogger<QuestionSessionManager> logger)
        {
            _logger = logger;
        }

        public string GetNewQuestionSession(int Amount)
        {
            string sessionId = Guid.NewGuid().ToString();
            List<QuestionModel> questions = FetchQuestions(Amount);
            _sessions[sessionId] = questions;
            return sessionId;
        }

        public List<QuestionModel> FetchQuestions(int Amount)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{ApiUrl}?amount={Amount}").Result;
                if (response.IsSuccessStatusCode) //todo fix this part
                {
                }
                else
                {
                    _logger.LogError($"Failed to fetch questions: {response.StatusCode}");
                    return new List<QuestionModel>();
                }
            }
        }
    }
}

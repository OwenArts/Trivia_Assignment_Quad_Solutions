using Newtonsoft.Json;
using Trivia_Assignment_backend.Models;
using Trivia_Assignment_backend.Models.Reponses;

namespace Trivia_Assignment_backend.Managers
{
    public class QuestionSessionManager
    {
        private readonly ILogger<QuestionSessionManager> _logger;
        private readonly Dictionary<string, List<QuestionModel>> _sessions = new Dictionary<string, List<QuestionModel>>();
        private readonly Dictionary<Guid, string> _questions = new Dictionary<Guid, string>();
        private readonly string ApiUrl = "https://opentdb.com/api.php";
        private readonly HttpClient _httpClient = new HttpClient();

        public QuestionSessionManager(ILogger<QuestionSessionManager> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<List<QuestionModel>> FetchQuestionsAsync(int Amount) 
        {
            using (_httpClient)
            {
                var response = await _httpClient.GetAsync($"{ApiUrl}?amount={Amount}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return TransformQuestions(content);
                }
                else
                {
                    _logger.LogError($"Failed to fetch questions: {response.StatusCode}");
                    return new List<QuestionModel>();
                }
            }
        }

        private List<QuestionModel> TransformQuestions(String RawAPIResult)
        {
            if (RawAPIResult == null)
                throw new ArgumentNullException(nameof(RawAPIResult));


            var questionsResponse = JsonConvert.DeserializeObject<RawQuestionsAPIResponse>(RawAPIResult);
            questionsResponse = questionsResponse ?? throw new InvalidOperationException("Deserialization resulted in null");

            List<QuestionModel> questionModels = new List<QuestionModel>();
            foreach (var question in questionsResponse.Results)
            {
                if (question == null)
                    continue;
                Guid QuestionId = Guid.NewGuid();

                var questionModel = new QuestionModel(question.type, question.question, question.correct_answer, question.incorrect_answers, QuestionId);
                questionModels.Add(questionModel);

                RegisterQuestion(question.correct_answer, QuestionId);
            }

            return questionModels;
        }

        private bool RegisterQuestion(string correctAnser, Guid questionId)
        {
            if (string.IsNullOrEmpty(correctAnser))
                throw new ArgumentException("Correct answer cannot be null or empty.", nameof(correctAnser));

            _questions[questionId] = correctAnser;
            return true;
        }

        public AnswerModel CheckSingleAnswers(AnswerModel answer)
        {
            if (!_questions.ContainsKey(answer.QuestionId))
                throw new ArgumentException("QuestionNotRegistered");

            answer.CorrectAnswer = _questions[answer.QuestionId];
            answer.WasAnswerCorrect = answer.CorrectAnswer.Equals(answer.GivenAnswer, StringComparison.OrdinalIgnoreCase);

            _questions.Remove(answer.QuestionId);

            return answer;
        }
    }
}

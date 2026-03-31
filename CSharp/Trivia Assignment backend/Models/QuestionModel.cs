namespace Trivia_Assignment_backend.Models
{
    public class QuestionModel
    {
        public Guid QuestionId { get; set; }
        public string Question { get; set; } = string.Empty;
        public List<string> Answers { get; set; } = new List<string>();
        private string QuestionType { get; set; } = string.Empty;

        public QuestionModel(string QuestionType, string Question, string CorrectAnswer, List<string> IncorrectAnswers, Guid QuestionId) { 
            this.QuestionType = QuestionType;
            this.Question = Question;
            this.QuestionId = QuestionId;

            var allAnswers = new List<string>(IncorrectAnswers);
            allAnswers.Add(CorrectAnswer);

            Random random = new Random();
            this.Answers = allAnswers.OrderBy(x => random.Next()).ToList();
        }
    }
}

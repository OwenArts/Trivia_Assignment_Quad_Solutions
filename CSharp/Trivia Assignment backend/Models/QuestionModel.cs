namespace Trivia_Assignment_backend.Models
{
    public class QuestionModel
    {
        public int QuestionIndex { get; set; }
        public string Question { get; set; } = string.Empty;
        public List<String> Answers { get; set; } = new List<string>();
        private string CorrectAnswer { get; set; } = string.Empty;
        private string QuestionType { get; set; } = string.Empty;

        public QuestionModel(string QuestionType, string Question, string CorrectAnswer, List<string> IncorrectAnswers) { 
            this.QuestionType = QuestionType;
            this.Question = Question;
            this.CorrectAnswer = CorrectAnswer;
            this.Answers.AddRange(IncorrectAnswers);
            this.Answers.Add(CorrectAnswer);
        }

        public bool CheckAnswer(string Answer)
        {
            return Answer == CorrectAnswer;
        }
    }
}

namespace Trivia_Assignment_backend.Models
{
    public class AnswerModel
    {
        public Guid QuestionId { get; set; }
        public string CorrectAnswer { get; set; }
        public string GivenAnswer { get; set; }
        public bool WasAnswerCorrect { get; set; }

        public AnswerModel() { }
        public AnswerModel(Guid questionId, string givenAnswer, string correctAnswer = "", bool wasAnswerCorrect = false)
        {
            QuestionId = questionId;
            GivenAnswer = givenAnswer;

            CorrectAnswer = correctAnswer;
            WasAnswerCorrect = wasAnswerCorrect;
        }
    }
}

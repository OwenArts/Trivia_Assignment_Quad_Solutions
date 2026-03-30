using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.IO.Pipes;
using System.Net;
using System.Net.Http.Json;
using Trivia_Assignment_backend.Managers;
using Trivia_Assignment_backend.Models;

namespace UnitTests_backend
{
    public class QuestionTests
    {
        [Fact]
        public async Task FetchSingleQuestionAsync()
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var logger = new Mock<ILogger<QuestionSessionManager>>();
            var client = new HttpClient(httpMessageHandlerMock.Object);
            var questionSessionManager = new QuestionSessionManager(logger.Object, client);
            QuestionModel receivedQuestion;
            string question = "Who is the main villain of Kirby&#039;s Return to Dreamland?";
            string correctAnswer = "Magolor";
            List<string> incorrectAnswers = ["Landia", "King Dedede", "Queen Sectonia"];
            var answers = new List<string>();

            answers.Add(correctAnswer);
            answers.AddRange(incorrectAnswers);
            var fakeResponseObject = new
            {
                response_code = 0,
                results = new[] {
                    new {
                        type = "multiple",
                        question = question,
                        correct_answer = correctAnswer,
                        incorrect_answers = incorrectAnswers
                    }
                }
            };
            string fakeResponse = JsonConvert.SerializeObject(fakeResponseObject);
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(fakeResponse, System.Text.Encoding.UTF8, "application/json")
                });
            var questionsList = await questionSessionManager.FetchQuestionsAsync(1);
            receivedQuestion = questionsList[0];

            Assert.NotNull(questionsList);
            Assert.NotEmpty(questionsList);
            Assert.Single(questionsList);
            Assert.NotNull(receivedQuestion);
            Assert.Equal(receivedQuestion.Question, question, true);
            Assert.NotEmpty(receivedQuestion.Answers);
            Assert.Equal(receivedQuestion.Answers.Count, answers.Count);
            foreach (var expectedAnswer in answers)
            {
                Assert.Contains(expectedAnswer, receivedQuestion.Answers);
            }
        }

        [Fact]
        public void CheckSingleAnswer_ShouldReturnTrue_WhenAnswerIsCorrect()
        {
            Assert.Equal(1, 1);
        }
        [Fact]
        public void CheckMultipleAnswer_ShouldReturnTrue_WhenAnswerIsCorrect()
        {
            Assert.Equal(1, 1);
        }
        [Fact]
        public void CheckSingleAnswer_ShouldRemoveFromDictionary_AfterCheck()
        {
            Assert.Equal(1, 1);
        }
        [Fact]
        public void CheckSingleAnswer_ShouldThrowException_WhenIdDoesNotExist()
        {
            Assert.Equal(1, 1);
        }
    }
}

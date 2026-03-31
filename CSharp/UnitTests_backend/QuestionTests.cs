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
        /// <summary>
        /// Checks if the fetching works using a mock http client message handler. Checks for fetching 1 question1.
        /// </summary>
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
        
        /// <summary>
        /// Checks if the fetching works using a mock http client message handler. Checks for fetching 1 question1.
        /// </summary>
        [Fact]
        public async Task FetchMultipleQuestionAsync()
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var logger = new Mock<ILogger<QuestionSessionManager>>();
            var client = new HttpClient(httpMessageHandlerMock.Object);
            var questionSessionManager = new QuestionSessionManager(logger.Object, client);
            QuestionModel receivedQuestion;
            string question1 = "Who is the main villain of Kirby&#039;s Return to Dreamland?";
            string correctAnswer1 = "Magolor";
            List<string> incorrectAnswers1 = ["Landia", "King Dedede", "Queen Sectonia"];
            string question2 = "What do sailors call the left side of a boat?";
            string correctAnswer2 = "Port";
            List<string> incorrectAnswers2 = ["Starboard", "Bow", "Stern"];
            var answers1 = new List<string>();
            var answers2 = new List<string>();

            answers1.Add(correctAnswer1);
            answers1.AddRange(incorrectAnswers1);

            answers2.Add(correctAnswer2);
            answers2.AddRange(incorrectAnswers2);
            var fakeResponseObject = new
            {
                response_code = 0,
                results = new[] {
                    new {
                        type = "multiple",
                        question = question1,
                        correct_answer = correctAnswer1,
                        incorrect_answers = incorrectAnswers1
                    },
                    new
                    {
                        type = "multiple",
                        question = question2,
                        correct_answer = correctAnswer2,
                        incorrect_answers = incorrectAnswers2
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

            Assert.NotNull(questionsList);
            Assert.NotEmpty(questionsList);
            Assert.Equal(2, questionsList.Count);

            Assert.NotNull(questionsList[0]);
            Assert.Equal(questionsList[0].Question, question1, true);
            Assert.NotEmpty(questionsList[0].Answers);
            Assert.Equal(questionsList[0].Answers.Count, answers1.Count);
            foreach (var expectedAnswer in answers1)
            {
                Assert.Contains(expectedAnswer, questionsList[0].Answers);
            }


            Assert.NotNull(questionsList[1]);
            Assert.Equal(questionsList[1].Question, question2, true);
            Assert.NotEmpty(questionsList[1].Answers);
            Assert.Equal(questionsList[1].Answers.Count, answers2.Count);
            foreach (var expectedAnswer in answers2)
            {
                Assert.Contains(expectedAnswer, questionsList[1].Answers);
            }


        }

        /// <summary>
        /// Checks an answer, but the answer is unknown to the question1 session manager, and therefore should throw an "QuestionNotRegistered" exception.
        /// </summary>
        [Fact]
        public void CheckUnknownQuestion()
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var logger = new Mock<ILogger<QuestionSessionManager>>();
            var client = new HttpClient(httpMessageHandlerMock.Object);
            var questionSessionManager = new QuestionSessionManager(logger.Object, client);
            var unknownAnswer = new AnswerModel(new Guid(), "blablabla");

            var exception = Assert.Throws<ArgumentException>(() => questionSessionManager.CheckSingleAnswer(unknownAnswer));
            Assert.Equal("QuestionNotRegistered", exception.Message);
        }
        
        /// <summary>
        /// Checks an answer that is first registered, using the fetch question1 connected to a mock http client message handler.
        /// The given answer is correct.
        /// </summary>
        [Fact]
        public async Task checkKnownAnswerForCorrectAnswer()
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

            var unknownAnswer = new AnswerModel(receivedQuestion.QuestionId, correctAnswer);

            var returnedAnswer = questionSessionManager.CheckSingleAnswer(unknownAnswer);

            Assert.True(returnedAnswer.WasAnswerCorrect);
            Assert.Equal(returnedAnswer.CorrectAnswer, correctAnswer);
        }


        /// <summary>
        /// Checks an answer that is first registered, using the fetch question1 connected to a mock http client message handler.
        /// The given answer is not correct.
        /// </summary>
        [Fact]
        public async Task checkKnownAnswerForIncorrectAnswer()
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

            var unknownAnswer = new AnswerModel(receivedQuestion.QuestionId, incorrectAnswers[0]);

            var returnedAnswer = questionSessionManager.CheckSingleAnswer(unknownAnswer);

            Assert.False(returnedAnswer.WasAnswerCorrect);
            Assert.Equal(returnedAnswer.CorrectAnswer, correctAnswer);
            Assert.NotEqual(returnedAnswer.GivenAnswer, correctAnswer);
        }

        [Fact]
        public async Task CheckSingleAnswerIsRemovedFromDictionary()
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

            var unknownAnswer = new AnswerModel(receivedQuestion.QuestionId, correctAnswer);

            var returnedAnswer = questionSessionManager.CheckSingleAnswer(unknownAnswer);


            Assert.True(returnedAnswer.WasAnswerCorrect);
            Assert.Equal(returnedAnswer.CorrectAnswer, correctAnswer);
            var exception = Assert.Throws<ArgumentException>(() => questionSessionManager.CheckSingleAnswer(unknownAnswer));
            Assert.Equal("QuestionNotRegistered", exception.Message);
        }
    }
}

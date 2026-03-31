namespace Trivia_Assignment_backend.Models.Reponses
{
    public class RawQuestionsAPIResponse
    {
        public int ResponseCode { get; set; }
        public List<RawQuestionsModel> Results { get; set; }
    }
}

namespace TriviaGame.Models
{
    public class QuestionsApiResponseJsonModel
    {
        public int? response_code { get; set; }
        public List<ResultModel>? results { get; set; } = new List<ResultModel>(); 
    }
}

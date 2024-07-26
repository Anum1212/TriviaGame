namespace TriviaGame.Models
{
    public class ResultModel
    {
        public int? questionNumber { get; set; } // This is a custom property that I added to the model
        public string? type { get; set; }
        public string? difficulty { get; set; }
        public string? category { get; set; }
        public string? question { get; set; }
        public string? correct_answer { get; set; }
        public List<string>? incorrect_answers { get; set; }
        public List<string>? multipleChoice { get; set; } // This is a custom property that I added to the model
    }
}

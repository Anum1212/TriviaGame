using TriviaGame.Models;


namespace TriviaGame.Business.Interfaces
{
    public interface IOpenTrivia
    {
        Task<QuestionsApiResponseJsonModel> GetTriviaQuestionCategoriesApiResponse();
        Task<QuestionsApiResponseJsonModel> GetTriviaQuestionDetailsApiResponse();
    }
}

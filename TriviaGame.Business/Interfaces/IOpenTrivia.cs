using TriviaGame.Models;
using System.Threading.Tasks;


namespace TriviaGame.Business.Interfaces
{
    public interface IOpenTrivia
    {
        Task<QuestionsApiResponseJsonModel> GetTriviaQuestionCategoriesApiResponse();
        Task<QuestionsApiResponseJsonModel> GetTriviaQuestionDetailsApiResponse();
    }
}

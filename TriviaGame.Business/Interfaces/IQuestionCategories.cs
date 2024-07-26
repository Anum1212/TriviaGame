using TriviaGame.Models;

namespace TriviaGame.Business.Interfaces
{
    public interface IQuestionCategories
    {
        public Task<CategoryApiResponseModel> GetTriviaQuestionCategoriesApiResponse();
        public List<CategoryModel> GetTriviaQuestionCategories();
    }
}

using TriviaGame.Models;

namespace TriviaGame.Business.Interfaces
{
    public interface ISession
    {
        public void StoreInSession(string key, object value);
         public List<ResultModel> GetQuestionDetailsFromSession();
        public List<CategoryModel> GetQuestionCategoriesFromSession();
        public bool CheckHasPlayerAnsweredTrivia();
        public bool CheckForSessionKey(string sessionKey);
    }
}

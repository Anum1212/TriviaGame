using Microsoft.AspNetCore.Http;
using TriviaGame.Models;
using System.Text;
using System.Text.Json;

namespace TriviaGame.Business
{
    public class SessionManager : Interfaces.ISession
    {
        private readonly IHttpContextAccessor _httpContext;
        public SessionManager(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        public void StoreInSession(string key, object value)
        {
            _httpContext.HttpContext.Session.Set(key, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)));
        }
        public List<ResultModel> GetQuestionDetailsFromSession()
        {
            string serializedValue = _httpContext.HttpContext.Session.GetString("QuestionDetails");
            if (serializedValue != null)
            {
                return JsonSerializer.Deserialize<List<ResultModel>>(serializedValue);
            }
            else
            {
                return new List<ResultModel>();
            }
        }
        public List<CategoryModel> GetQuestionCategoriesFromSession()
        {
            string serializedValue = _httpContext.HttpContext.Session.GetString("QuestionCategories");
            if (serializedValue != null)
            {
                return JsonSerializer.Deserialize<List<CategoryModel>>(serializedValue);
            }
            else
            {
                return new List<CategoryModel>();
            }
        }
        public bool CheckHasPlayerAnsweredTrivia()
        {
            return bool.Parse(_httpContext.HttpContext.Session.GetString("HasUserAnsweredTrivia"));
        }
        public bool CheckForSessionKey(string sessionKey)
        {
            foreach (var key in _httpContext.HttpContext.Session.Keys)
            {
                if (key.ToString() == sessionKey)
                    return true;
            }
            return false;
        }

    }
}

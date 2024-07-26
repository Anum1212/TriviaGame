using TriviaGame.Business.Interfaces;
using TriviaGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TriviaGame.Business
{
    public class QuestionCategoriesManager : IQuestionCategories
    {
        private readonly Interfaces.ISession _session;
        private readonly HttpClient _questionCategoriesApi;

        public QuestionCategoriesManager(Interfaces.ISession session)
        {
            _questionCategoriesApi = new HttpClient()
            {
                BaseAddress = new Uri("https://opentdb.com/api_category.php")
            };
            _session = session;
        }

        public async Task<CategoryApiResponseModel> GetTriviaQuestionCategoriesApiResponse()
        {
            var result = new CategoryApiResponseModel();
            var response = await _questionCategoriesApi.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();

                result = JsonSerializer.Deserialize<CategoryApiResponseModel>(stringResponse);
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }

            return result ?? new CategoryApiResponseModel();
        }

        public List<CategoryModel> GetTriviaQuestionCategories()
        {
            if (!_session.CheckForSessionKey("QuestionCategories"))
            {
                List<CategoryModel>? categoryModel = new List<CategoryModel>();
                categoryModel = GetTriviaQuestionCategoriesApiResponse().Result.trivia_categories;
                _session.StoreInSession("QuestionCategories", categoryModel);
                return categoryModel;
            }
            else
            {
                return _session.GetQuestionCategoriesFromSession();
            }
        }

        
    }
}

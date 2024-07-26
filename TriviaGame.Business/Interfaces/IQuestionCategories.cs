using TriviaGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaGame.Business.Interfaces
{
    public interface IQuestionCategories
    {
        public Task<CategoryApiResponseModel> GetTriviaQuestionCategoriesApiResponse();
        public List<CategoryModel> GetTriviaQuestionCategories();
    }
}

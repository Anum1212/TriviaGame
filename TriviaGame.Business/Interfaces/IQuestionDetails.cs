using TriviaGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaGame.Business.Interfaces
{
    public interface IQuestionDetails
    {
        public Task<QuestionsApiResponseJsonModel> GetTriviaQuestionDetailsApiResponse(string category, string difficulty);
        public List<ResultModel> GetTriviaQuestions(string category, string difficulty);
        public AnswerModel GetTriviaCorrectAnswer(int questionNumber, string userChoice);
        public Uri CreateOpenTriviaUri(string category, string difficulty);
    }
}

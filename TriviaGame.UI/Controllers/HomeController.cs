using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TriviaGame.Business.Interfaces;
using TriviaGame.Models;
using TriviaGame.UI.Models;

namespace TriviaGame.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuestionCategories _questionCategories;
        private readonly IQuestionDetails _questionDetails;

        public HomeController(IQuestionCategories questionCategories, IQuestionDetails questionDetails)
        {
            _questionCategories = questionCategories;
            _questionDetails = questionDetails;
        }

        public IActionResult Index()
        {
            List<CategoryModel> categoryModel = _questionCategories.GetTriviaQuestionCategories();
            return View(categoryModel);
        }

        public IActionResult InitializeTrivia(string category, string difficulty)
        {
            List<ResultModel> triviaQuestionsList = _questionDetails.GetTriviaQuestions(category, difficulty);
            return Json(triviaQuestionsList);
        }
        public AnswerModel CheckAnswer(int questionNumber, string answerSelected)
        {
            AnswerModel answerModel = _questionDetails.GetTriviaCorrectAnswer(questionNumber, answerSelected);
            return answerModel;
        }
    }
}

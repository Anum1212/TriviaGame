using Microsoft.AspNetCore.Http;
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
    public class QuestionDetailsManager : IQuestionDetails
    {
        private readonly Interfaces.ISession _session;
        private int questionCategory;
        private string questionDifficulty = string.Empty;
        private string questionType = string.Empty;

        public QuestionDetailsManager(Interfaces.ISession session)
        {
            _session = session;
        }

        public async Task<QuestionsApiResponseJsonModel> GetTriviaQuestionDetailsApiResponse(string category, string difficulty)
        {
            var result = new QuestionsApiResponseJsonModel();


            Uri openTriviaUri = CreateOpenTriviaUri(category, difficulty);

            HttpClient _questionDetailsApi = new HttpClient()
            {
                BaseAddress = openTriviaUri
            };
            var response = await _questionDetailsApi.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();

                result = JsonSerializer.Deserialize<QuestionsApiResponseJsonModel>(stringResponse);
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }

            return result ?? new QuestionsApiResponseJsonModel();
        }
        public List<ResultModel> GetTriviaQuestions(string category, string difficulty)
        {
            //check if the session has the question details
            if (_session.CheckForSessionKey("QuestionDetails"))
            {
                // check if the user has answered any trivia questions
                // if they haven't, then return the question details from the session
                // HasUserAnsweredTrivia session key will only exist if the user answered a trivia question so no need to check for the value
                if (!_session.CheckForSessionKey("HasUserAnsweredTrivia"))
                {
                    return _session.GetQuestionDetailsFromSession();
                }
                else
                {
                    // if the user has answered the trivia questions, then return a new set of questions to prevent the user from cheating by answering the same questions again
                    return GetTriviaQuestionnaire(category, difficulty);
                }
            }
            // if the session does not have any question, then return a new set of questions from the API
            else
            {
                return GetTriviaQuestionnaire(category, difficulty);
            }
        }

        private List<ResultModel> GetTriviaQuestionnaire(string category, string difficulty)
        {
            List<ResultModel>? resultModels = new List<ResultModel>();
            resultModels = GetTriviaQuestionDetailsApiResponse(category, difficulty).Result.results;
            if (resultModels != null)
            {
                int questionNumber = 0;
                foreach (ResultModel result in resultModels)
                {
                    questionNumber++;
                    result.questionNumber = questionNumber;
                    List<string> answers = new List<string>();
                    answers.Add(result.correct_answer);
                    answers.AddRange(result.incorrect_answers);
                    result.multipleChoice = answers.OrderBy(a => Guid.NewGuid()).ToList();
                    result.question = WebUtility.HtmlDecode(result.question);
                    result.correct_answer = WebUtility.HtmlDecode(result.correct_answer);
                    for (int i = 0; i < result.multipleChoice.Count; i++)
                    {
                        result.multipleChoice[i] = WebUtility.HtmlDecode(result.multipleChoice[i]);
                    }
                }
            }
            _session.StoreInSession("QuestionDetails", resultModels);
            return resultModels;
        }

        public AnswerModel GetTriviaCorrectAnswer(int questionNumber, string userChoice)
        {
            questionNumber--; // to get the correct index
            AnswerModel answerModel = new AnswerModel();
            answerModel.answer = _session.GetQuestionDetailsFromSession()[questionNumber].correct_answer;
            if (answerModel.answer == userChoice)
            {
                answerModel.isCorrect = true;
            }
            else
            {
                answerModel.isCorrect = false;
            }
            _session.StoreInSession("HasUserAnsweredTrivia", true);
            return answerModel;
        }
        public Uri CreateOpenTriviaUri(string category, string difficulty)
        {
            string baseUrl = "https://opentdb.com/api.php";
            UriBuilder uriBuilder = new UriBuilder(baseUrl);
            uriBuilder.Query = $"amount=20&category={category}&difficulty={difficulty}";

            return uriBuilder.Uri;
        }
    }

}


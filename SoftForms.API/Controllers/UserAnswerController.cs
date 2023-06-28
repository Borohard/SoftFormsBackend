using Microsoft.AspNetCore.Mvc;
using SoftForms.Data.Repositories;
using SoftForms.Model.Entities;

namespace SoftForms.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAnswerController : ControllerBase
    {
        UserAnswerRepository _userAnswerRepository { get; set; }

        private readonly ILogger<UserAnswerController> _logger;

        SurveyRepository _surveyRepository { get; set; }

        public UserAnswerController(ILogger<UserAnswerController> logger, UserAnswerRepository userAnswerRepository, SurveyRepository surveyRepository)
        {
            _userAnswerRepository = userAnswerRepository;
            _logger = logger;
            _surveyRepository = surveyRepository;
        }


        [HttpPost]
        public Guid Add([FromBody] UserQuestionAnswer userQuestionAnswer)
        {
            userQuestionAnswer.Id = Guid.NewGuid();
            userQuestionAnswer.Created = DateTime.Now;
            _userAnswerRepository.Add(userQuestionAnswer);
            return userQuestionAnswer.Id;
        }

        [HttpGet("Answers/")]
        public List<UserQuestionAnswer> GetAnswersBySurveyId([FromQuery] Guid surveyId)
        {
            return _userAnswerRepository.GetAllBySurveyId(surveyId);           
        }

        //[HttpGet]
        //public Survey GetNextQuestions([FromBody] List<UserQuestionAnswer> questionAnswers, [FromQuery] Guid surveyId)
        //{
        //    var survey = _surveyRepository.Get(surveyId);
        //    var newSurvey = new Survey
        //    {
        //        Id = survey.Id,
        //        Title = survey.Title,
        //        Description = survey.Description,
        //        Questions = new List<Question>()

        //    };

        //    foreach (var question in survey.Questions)
        //    {
        //        if (question.ShowConditions == null || question.ShowConditions.Count == 0)
        //        {
        //            newSurvey.Questions.Add(question);
        //            continue;
        //        }

        //        var isQuestionMatch = true;

        //        foreach (var condition in question.ShowConditions)
        //        {
        //            var userAnswer = questionAnswers.SingleOrDefault(x => x.QuestionId == condition.QuestionId);
                    
        //            if (userAnswer == null || !userAnswer.AnswerValues.ToHashSet().SetEquals(condition.QuestionValue.ToHashSet()))
        //            {
        //                isQuestionMatch = false;
        //                break;
        //            }                    
        //        }

        //        if (isQuestionMatch) newSurvey.Questions.Add(question);              
        //    }

        //    return newSurvey;
        //} 

    }
}

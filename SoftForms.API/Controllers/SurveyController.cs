using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.Servers;
using SoftForms.API.Services;
using SoftForms.Data.Repositories;
using SoftForms.Model.Entities;
using ZstdSharp;

namespace SoftForms.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SurveyController : ControllerBase
    {
        SurveyRepository _surveyRepository { get; set; }

        private readonly ILogger<SurveyController> _logger;

        public SurveyController(ILogger<SurveyController> logger, SurveyRepository surveyRepository)
        {
            _surveyRepository = surveyRepository;
            _logger = logger;
        }

        [HttpPost]
        public Guid Add([FromBody] Survey survey)
        {
            survey.Id = Guid.NewGuid();
            survey.Created = DateTime.Now;
            survey.Updated = DateTime.Now;
            _surveyRepository.Add(survey);
            return survey.Id;
        }


        [HttpGet("Create/")]
        [Authorize]
        public Guid Create()
        {
            var userId = Guid.Parse(HttpContext.User.GetUserId());
            var survey = new Survey()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                Updated = DateTime.Now,
                UserId = userId,
                Title = "Безымянный опрос",
                Description = "",
                Pages = new List<Page>()
                {
                    new Page()
                    {
                        Id = Guid.NewGuid(),                          
                        Questions = new List<Question>()
                        {
                            new Question()
                            {
                                Title = "Пустой вопрос",
                                Id = Guid.NewGuid(),
                                QuestionType = QuestionType.Text,
                                IsRequired = false,
                                Options = new List<Option>()
                            }
                        }
                    }
                }
                
            };
            _surveyRepository.Add(survey);
            return survey.Id;
        }



        [HttpGet]
        [Authorize]
        public List<Survey> GetAll()
        {
            var userId = HttpContext.User.GetUserId();
            if (userId != null)
            {
                var surveys = _surveyRepository.GetAll(Guid.Parse(userId));
                if (surveys == null)
                {
                    return new List<Survey>();
                }
                return surveys;
            }
            throw new Exception(userId);
            
        }

        [HttpGet("SurveySearch/")]
        public List<Survey> GetSurveysBySubstr([FromQuery] Guid userId, [FromQuery] string subStr)
        {
            var surveys = _surveyRepository.GetAll(userId);
            if (surveys == null)
            {
                return new List<Survey>();
            }
            var filter = new List<Survey>();
            foreach (var survey in surveys)
            {
                if (survey.Title != null & survey.Title.Contains(subStr)) {
                    filter.Add(survey);
                }               
            }
            return filter;
        }


        [HttpGet("{surveyId:Guid}")]
        [Authorize]
        public Survey Get([FromRoute] Guid surveyId)
        {
            var userId = HttpContext.User.GetUserId();
            var isUserIsOwner = CheckAccess(_surveyRepository.Get(surveyId), Guid.Parse(userId));
            if (isUserIsOwner)
            {
                return _surveyRepository.Get(surveyId);
            }
            return null;
        }

        [HttpDelete]
        [Authorize]
        public void Delete([FromQuery] Guid surveyId)
        {
            var userId = HttpContext.User.GetUserId();
            var isUserIsOwner = CheckAccess(_surveyRepository.Get(surveyId), Guid.Parse(userId));
            if (isUserIsOwner)
            {
                _surveyRepository.Delete(surveyId);
            }          
        }

        [HttpPut]
        [Authorize]
        public Survey Update([FromBody] Survey survey)
        {
            var userId = Guid.Parse(HttpContext.User.GetUserId());
            var isUserIsOwner = CheckAccess(_surveyRepository.Get(survey.Id), userId);
            if (isUserIsOwner)
            {
                survey.UserId = userId;
                survey.Updated = DateTime.Now;
                return _surveyRepository.Update(survey);
            }
            return null;
            
        }

        bool CheckAccess(Survey survey, Guid userId)
        {
            return survey.UserId == userId;
        }
    }
}
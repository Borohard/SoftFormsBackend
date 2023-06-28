using MongoDB.Driver;
using SoftForms.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftForms.Data.Repositories
{
    public class UserAnswerRepository
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoCollection<UserQuestionAnswer> _answers;
        public UserAnswerRepository(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;

            var answerCollection = _mongoClient.GetDatabase("SoftForms").GetCollection<UserQuestionAnswer>("Answers");

            _answers = answerCollection;
        }

        public void Add(UserQuestionAnswer answer)
        {
            _answers.InsertOne(answer);
        }
        public void Delete(Guid answerId)
        {
            _answers.DeleteOne(x => x.Id == answerId);
        }
        public UserQuestionAnswer Update(UserQuestionAnswer answer)
        {
            _answers.ReplaceOne(x => x.Id == answer.Id, answer);
            return answer;
        }
        public void Save(UserQuestionAnswer answer)
        {
            if (_answers.Find(x => x.Id == answer.Id).Any()) Update(answer);
                else Add(answer);
        }
        public List<UserQuestionAnswer> GetAll(Guid userId)
        {
            var answers = _answers.Find(x => x.UserId == userId).ToList();
            return answers;
        }

        public UserQuestionAnswer Get(Guid answerId)
        {
            return _answers.Find(x => x.Id == answerId).SingleOrDefault();
        }

        public List<UserQuestionAnswer> GetAllBySurveyId(Guid surveyId)
        {
            var answers = _answers.Find(x => x.SurveyId == surveyId).ToList();
            return answers;
        }



        

    }
}

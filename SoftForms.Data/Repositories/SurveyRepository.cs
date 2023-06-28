using MongoDB.Driver;
using SoftForms.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftForms.Data.Repositories
{
    public class SurveyRepository
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoCollection<Survey> _surveys;
        public SurveyRepository(MongoClient mongoClient ) {
            _mongoClient = mongoClient;
            
            var surveyCollection = _mongoClient.GetDatabase("SoftForms").GetCollection<Survey>("surveys");

            _surveys = surveyCollection;
        }

        public void Add(Survey survey)
        {
            _surveys.InsertOne(survey);
        }
        public void Delete(Guid surveyId)
        {
            _surveys.DeleteOne(x => x.Id == surveyId);
        }
        public Survey Update(Survey survey)
        {
            _surveys.ReplaceOne(x => x.Id == survey.Id, survey);
            return survey;
        }
        public List<Survey> GetAll(Guid userId) 
        {
            var surveys = _surveys.Find(x => x.UserId == userId).ToList();
            return surveys;
        }

        public Survey Get(Guid surveyId)
        {
            return _surveys.Find(x => x.Id == surveyId).SingleOrDefault();
        }
    }
}

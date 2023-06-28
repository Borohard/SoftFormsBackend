 using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftForms.Model.Entities
{
    public class UserQuestionAnswer
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SurveyId { get; set; }
        public Guid QuestionId { get; set; }
        public List<string>? AnswerValues { get; set;}  
        public DateTime? Created { get; set; }
    }
}

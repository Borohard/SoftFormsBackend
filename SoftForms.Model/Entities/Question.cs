using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftForms.Model.Entities
{
    public class Question
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }

        [BsonRepresentation(BsonType.String)]
        public QuestionType QuestionType { get; set; }
        public bool IsRequired { get; set; }
        public List<Option>? Options { get; set; }
        public List<QuestionMatchingCondition>? ShowConditions { get; set; }
        public List<QuestionMatchingCondition>? HideConditions { get; set; }
    }
}

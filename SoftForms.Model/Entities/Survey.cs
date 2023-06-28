using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftForms.Model.Entities
{
    public class Survey
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }  
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<Page>? Pages { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set;}
    }
}

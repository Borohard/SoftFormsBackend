using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftForms.Model.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Login { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}

using MongoDB.Driver;
using SoftForms.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftForms.Data.Repositories
{
    public class UserRepository
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoCollection<User> _users;
        public UserRepository(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;

            var userCollection = _mongoClient.GetDatabase("SoftForms").GetCollection<User>("users");

            _users = userCollection;
        }

        public void Add(User user)
        {
            _users.InsertOne(user);
        }

        public User GetByEmail(String email)
        {
            return _users.Find(x => x.Email == email).SingleOrDefault();
        }

    }
}

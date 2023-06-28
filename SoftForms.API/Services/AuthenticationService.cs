using SoftForms.Data.Repositories;
using SoftForms.Model.Entities;

namespace SoftForms.API.Services
{
    public class AuthenticationService
    {
        UserRepository _userRepository;
        public AuthenticationService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Authenticate (string email, string password)
        {
            var user = _userRepository.GetByEmail(email);
            if (user == null)
            {
                return null;
            }
            if (user.Password == password)
            {
                return user;
            }
            return null;
        }
    }
}

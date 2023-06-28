using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SoftForms.API.Services;
using SoftForms.Data.Repositories;
using SoftForms.Model.Entities;
using SoftForms.Model.Requests;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SoftForms.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAuthenticationController : ControllerBase
    {
        AuthenticationService _authenticationService;
        UserRepository _userRepository;

        public UserAuthenticationController(AuthenticationService authenticationService, UserRepository userRepository)
        {
            _authenticationService = authenticationService;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route ("Register")]
        public IActionResult UserRegister([FromBody] User user)
        {
            
            if ( _userRepository.GetByEmail(user.Email) != null )
            {
                return BadRequest("Такой пользователь уже существует");    
            }

            var newUser = new User()
            {
                Id = Guid.NewGuid(),
                Login = user.Login,
                Email = user.Email,
                Password = user.Password
            };

            _userRepository.Add(newUser);
            return Ok(GenerateJWT(newUser));
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult UserLogin([FromBody] UserLoginRequest userLoginRequest)
        {
            User isUserExist = _authenticationService.Authenticate(userLoginRequest.Email, userLoginRequest.Password);
           
            string userToken = isUserExist != null ? GenerateJWT(isUserExist) : null;
            
            IActionResult userRequest = userToken != null ? Ok(userToken) : Unauthorized("Такого юзера не существует");
            return userRequest;          
        }

        [HttpGet]
        [Route ("CurrentUser")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var user = HttpContext.User;
            return Ok("test");
        }

        private string GenerateJWT(User user)
        {
            var issuer = "http://localhost:5267";
            var audience = "http://localhost:3000";
            var key = Encoding.ASCII.GetBytes("qweqweqweqweqweqweqweqweqweqweqweqweqweqweqweqweqweqweqweqwe");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                Expires = DateTime.UtcNow.AddMinutes(120),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;

        }
        
    }
}

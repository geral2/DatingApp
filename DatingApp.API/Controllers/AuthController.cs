using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserToRegisterDto userToRegister)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            userToRegister.username = userToRegister.username?.ToLower();

            if (await _repo.UserExists(userToRegister.username))
                return BadRequest($"Usename {userToRegister.username} already exists!");

            var newUser = new User()
            {
                UserName = userToRegister.username
            };

            var UserRegistered = await _repo.Register(newUser, userToRegister.password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserToLoginDto userToLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var UserLogged = await _repo.Login(userToLogin.username, userToLogin.password);

            if (UserLogged == null)
                return Unauthorized(); // BadRequest("User or password doesn't match!");
            
            // Payload: User info 
            var Claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, UserLogged.Id.ToString()),
                new Claim(ClaimTypes.Name, UserLogged.UserName)
            };

            // Generate SSK Key 
            var jkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSetting:Token").Value));

            //Signing key
            var creds = new SigningCredentials(jkey, SecurityAlgorithms.HmacSha512Signature);

            //Token Descriptor
            var tokenDescription = new SecurityTokenDescriptor()
            {
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds,
                    Subject = new ClaimsIdentity(Claims)
            };

        //     //  Finally create a Token
        //    var header = new JwtHeader(creds);

        //    //Some PayLoad that contain information about the  customer
        //    var payload = new JwtPayload
        //    {
        //        { "userId ", UserLogged.Id.ToString()},
        //        { "username", UserLogged.UserName},
        //    };

           //
        //    var secToken = new JwtSecurityToken(header, payload);

            var tokenHandler  = new JwtSecurityTokenHandler();
            
            // var token = tokenHandler.WriteToken(secToken);
            var token = tokenHandler.CreateToken(tokenDescription); 

            return Ok(new {
                token = tokenHandler.WriteToken(token) //token  
            });
        }

    }
}
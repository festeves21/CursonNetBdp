
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebAppDia2.Entities;
using WebAppDia2.Models;
using WebAppDia2.Services;

namespace WebAppDia2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase

    {

        private readonly JwtSettings _jwtSettings;

        public AuthController(IOptions<JwtSettings> jwtSettings)

        {

            _jwtSettings = jwtSettings.Value;

        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)

        {

            LoginServices logServ = new LoginServices();

            var user = logServ.AuthenticateUser(login);

            if (user == null)

                return Unauthorized();

            var tokenString = logServ.GetToken(user, login.ClientType, _jwtSettings);


            return Ok(new { Token = tokenString });

        }


    }
}

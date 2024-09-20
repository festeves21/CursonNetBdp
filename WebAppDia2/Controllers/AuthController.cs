
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebAppDia2.Entities;
using WebAppDia2.Models;
using WebAppDia2.Services;
using WebAppDia3.Models;

namespace WebAppDia2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase

    {
        private readonly JwtTokenService _jw;

        public AuthController(JwtTokenService jw)
        {
            _jw = jw;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            LoginServices logServ = new LoginServices();

            var user = logServ.AuthenticateUser(login);

            if (user == null)
                return Unauthorized();

            // Generar el Access Token y el Refresh Token
            var tokenResponse = await _jw.GenerateToken(user, login.ClientType);

            // Crear el objeto de respuesta usando la DTO
            var response = new AuthResponseDTO
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken.Token,
                RefreshTokenExpires = tokenResponse.RefreshToken.Expires
            };


            // Retornar la respuesta con el token y el refresh token
            return Ok(response);

        }



    }
}

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAppDia2.Entities;
using WebAppDia2.Models;

namespace WebAppDia2.Services
{
    public class LoginServices
    {
        public User AuthenticateUser(LoginModel log) 
        {
            // Aquí deberías autenticar al usuario con tu lógica de negocio
            // Este es solo un ejemplo de usuario
            return new User
            {
                Id = 1,
                UserName = "exampleUser",
                Role = "admin"
            };
        }

        public string GetToken(User user, string clientType, JwtSettings jwtSettings)
        {
            var claims = new[]
           {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim("client_type", clientType), // Claim específico para el tipo de cliente
            new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpiresInMinutes),
                SigningCredentials = creds,
                Issuer = jwtSettings.Issuer,
                Audience = jwtSettings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;

        }









    }
}

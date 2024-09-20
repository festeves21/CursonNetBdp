using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAppDia2.Entities;
using WebAppDia2.Models;
using WebAppDia3.Contract.Dtos;

namespace WebAppDia2.Services
{
    public class LoginServices
    {
        public UserDTO AuthenticateUser(LoginModel log) 
        {
            // Aquí deberías autenticar al usuario con tu lógica de negocio
            // Este es solo un ejemplo de usuario
            return new UserDTO
            {
                Id = 2,
                UserName = "exampleUser",
                Roles = "admin"
            };
        }




    }
}

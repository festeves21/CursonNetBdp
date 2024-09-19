using WebAppDia2.Entities;

namespace WebAppDia3.Models
{
    public class RefreshToken
    {

        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }

        public bool IsActive { get; set; } = true; // Para indicar si el token está activo
        public int UserId { get; set; } // Relación con el usuario
        public User User { get; set; } // Navegación a la entidad User

    }
}

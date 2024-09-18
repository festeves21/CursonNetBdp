namespace WebAppDia2.Models
{
    public class JwtSettings
    {

        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiresInMinutes { get; set; }
    }
}

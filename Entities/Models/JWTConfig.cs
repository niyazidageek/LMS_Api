using System;
namespace Entities.Models
{
    public class JWTConfig
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int DurationInMinutes { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}

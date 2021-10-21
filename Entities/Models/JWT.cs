using System;
namespace Entities.Models
{
    public class JWT
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string RefreshToken { get; set; }
    }
}

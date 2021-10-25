using System;
using System.Collections.Generic;

namespace Entities.DTOs
{
    public class LoginResponseDTO:ResponseDTO
    {
        public string Token { get; set; }

        public DateTime ExpiryDate { get; set; }

        public List<string> Roles { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Username { get; set; }
    }
}

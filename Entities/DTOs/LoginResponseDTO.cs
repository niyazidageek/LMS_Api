﻿using System;
namespace Entities.DTOs
{
    public class LoginResponseDTO:ResponseDTO
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
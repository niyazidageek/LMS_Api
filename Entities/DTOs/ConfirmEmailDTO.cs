using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class ConfirmEmailDTO
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
    }
}

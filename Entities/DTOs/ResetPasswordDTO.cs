using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class ResetPasswordDTO
    {
        [Required, StringLength(255), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required,StringLength(255)]
        public string NewPassword { get; set; }
    }
}

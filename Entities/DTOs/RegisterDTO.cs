using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class RegisterDTO
    {
        [Required, StringLength(255)]
        public string Name { get; set; }

        [Required, StringLength(255)]
        public string Surname { get; set; }

        [Required, StringLength(255)]
        public string Username { get; set; }

        [Required, StringLength(255), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, StringLength(155)]
        public string Password { get; set; }
    }
}

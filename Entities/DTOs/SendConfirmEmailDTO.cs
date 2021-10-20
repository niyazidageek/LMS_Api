using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class SendConfirmEmailDTO
    {
        [Required, StringLength(255), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}

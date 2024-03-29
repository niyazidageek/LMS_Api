﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class AddRoleDTO
    {
        [Required, StringLength(255), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, StringLength(255)]
        public string Role { get; set; }
    }
}

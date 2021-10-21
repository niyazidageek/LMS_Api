using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class AppUser: IdentityUser
    {
        [Required, StringLength(255)]
        public string Name { get; set; }

        [Required, StringLength(255)]
        public string Surname { get; set; }

        public ICollection<Subject> Subjects { get; set; }
    }
}

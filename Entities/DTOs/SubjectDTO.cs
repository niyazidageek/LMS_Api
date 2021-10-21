using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities.Models;

namespace Entities.DTOs
{
    public class SubjectDTO
    {
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }

        public ICollection<AppUser> Users { get; set; }
    }
}

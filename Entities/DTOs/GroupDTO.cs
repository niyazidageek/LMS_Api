using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities.Models;

namespace Entities.DTOs
{
    public class GroupDTO
    {
        public int Id { get; set; }

        [Required, StringLength(10)]
        public string Name { get; set; }

        [Required]
        public SubjectDTO Subject { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int AppUsersCount { get; set; }

        public List<AppUserDTO> AppUsers { get; set; }

        public AppUserDTO Teacher { get; set; }
    }
}

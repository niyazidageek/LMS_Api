using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entities.Models
{
    public class Group : IEntity
    {
        public int Id { get; set; }

        [Required, StringLength(10)]
        public string Name { get; set; }

        [Required]
        public Subject Subject { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public AppUser Teacher { get; set; }

        [Required]
        public ICollection<AppUser> Students { get; set; }
    }
}

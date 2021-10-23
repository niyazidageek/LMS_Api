using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class Group : IEntity
    {
        public int Id { get; set; }

        [Required, StringLength(10)]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public Subject Subject { get; set; }

        public List<AppUserGroup> AppUserGroups { get; set; }
    }
}

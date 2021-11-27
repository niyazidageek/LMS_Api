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
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public GroupMaxPoint GroupMaxPoint { get; set; }

        public List<AppUserGroup> AppUserGroups { get; set; }

        public List<Lesson> Lessons { get; set; }

        public List<Quiz> Quizzes { get; set; }
    }
}

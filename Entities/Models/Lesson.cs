using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Entities.Models
{
    public class Lesson:IEntity
    {
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int GroupId { get; set; }

        public bool IsOnline { get; set; }

        public string Description { get; set; }

        public Group Group { get; set; }

        public List<Assignment> Assignments { get; set; }

        public List<Theory> Theories { get; set; }

        public LessonJoinLink LessonJoinLink { get; set; }
    }
}

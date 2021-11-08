using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Entities.Models
{
    public class LessonAssignment : IEntity
    {
        public int Id { get; set; }

        public int LessonId { get; set; }

        public string FileName { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}

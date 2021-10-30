using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Entities.Models
{
    public class Question : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Point { get; set; }

        public int QuizId { get; set; }

        public Quiz Quiz { get; set; }

        public string FileName { get; set; }

        public List<Option> Options { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Entities.Models
{
    public class Option : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsCorrect { get; set; }

        public Material Material { get; set; }

        public Question Question { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

        public string ExistingFileName { get; set; }
    }
}

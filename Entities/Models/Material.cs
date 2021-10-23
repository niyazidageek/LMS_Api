using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entities.Models
{
    public class Material : IEntity
    {
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string FileName { get; set; }

        public List<LessonMaterial> LessonMaterials { get; set; }
    }
}

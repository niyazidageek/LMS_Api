using System;
using Core.Entities;

namespace Entities.Models
{
    public class LessonMaterial:IEntity
    {
        public int Id { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public int MaterialId { get; set; }
        public Material Material { get; set; }
    }
}

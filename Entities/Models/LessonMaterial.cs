using System;
using Core.Entities;

namespace Entities.Models
{
    public class LessonMaterial:IEntity
    {
        public int Id { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public string FileName { get; set; }
    }
}

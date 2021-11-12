using System;
using Core.Entities;

namespace Entities.Models
{
    public class Theory:IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}

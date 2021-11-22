using System;
using System.Collections.Generic;
using Core.Entities;

namespace Entities.Models
{
    public class Theory:IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public decimal Point { get; set; } = 0;

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public List<TheoryAppUser> TheoryAppUsers { get; set; }
    }
}

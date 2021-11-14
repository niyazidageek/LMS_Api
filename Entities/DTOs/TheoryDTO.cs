using System;
using System.Collections.Generic;

namespace Entities.DTOs
{
    public class TheoryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public decimal Point { get; set; }

        public List<TheoryAppUserDTO> TheoryAppUsers { get; set; }

        public int LessonId { get; set; }
        public LessonDTO Lesson { get; set; }
    }
}

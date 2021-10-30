using System;
namespace Entities.DTOs
{
    public class LessonMaterialDTO
    {
        public int Id { get; set; }

        public int LessonId { get; set; }
        public LessonDTO Lesson { get; set; }

        public string FileName { get; set; }
    }
}

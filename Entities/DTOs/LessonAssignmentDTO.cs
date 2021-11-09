using System;

namespace Entities.DTOs
{
    public class LessonAssignmentDTO
    {
        public int Id { get; set; }

        public int LessonId { get; set; }

        public string FileName { get; set; }

        //public DateTime DeadLine { get; set; }
    }
}
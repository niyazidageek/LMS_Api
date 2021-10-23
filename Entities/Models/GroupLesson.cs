using System;
namespace Entities.Models
{
    public class GroupLesson
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}

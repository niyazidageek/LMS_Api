using System;
using Core.Entities;

namespace Entities.Models
{
    public class LessonJoinLink:IEntity
    {
        public int Id { get; set; }

        public int LessonId { get; set; }
        public string JoinLink { get; set; }

        public Lesson Lesson { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Entities.DTOs
{
    public class TeacherHomeDTO
    {
        public List<AppUserDTO> Students { get; set; }

        public List<LessonDTO> Lessons { get; set; }

        public List<GroupDTO> Groups { get; set; }

        public int TotalTheories { get; set; }

        public int TotalAssignments { get; set; }

        public int CurrentGroupId { get; set; }

        public decimal MaxPoint { get; set; }
    }
}

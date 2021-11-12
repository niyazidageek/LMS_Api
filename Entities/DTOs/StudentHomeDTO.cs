using System;
using System.Collections.Generic;

namespace Entities.DTOs
{
    public class StudentHomeDTO
    {
        public AppUserDTO Teacher { get; set; }

        public int ProgressPercentage { get; set; }

        public int TotalAssignments { get; set; }

        public int SubmittedAssignmentsCount { get; set; }

        public int TotalTheories { get; set; }

        public int ReadTheoriesCount { get; set; }

        public decimal CurrentPoint { get; set; }

        public List<LessonDTO> Lessons { get; set; }

        public List<GroupDTO> Groups { get; set; }

        public int CurrentGroupId { get; set; }
    }
}

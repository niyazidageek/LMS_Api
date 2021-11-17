using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Entities.DTOs
{
    public class LessonDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int GroupId { get; set; }

        public string Description { get; set; }

        public bool IsOnline { get; set; }

        public GroupDTO Group { get; set; }

        public List<AssignmentDTO> Assignments { get; set; }

        public List<TheoryDTO> Theories { get; set; }

        public int Count { get; set; }
    }
}

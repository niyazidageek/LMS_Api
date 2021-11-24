using System;
using System.Collections.Generic;

namespace Entities.DTOs
{
    public class AssignmentDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        public DateTime CreationDate { get; set; }

        public int LessonId { get; set; }
        public LessonDTO Lesson { get; set; }

        public decimal MaxGrade { get; set; } = 0;

        public bool NotifyAll { get; set; }

        public List<AssignmentMaterialDTO> AssignmentMaterials { get; set; }

        public List<AssignmentAppUserDto> AssignmentAppUsers { get; set; }
    }
}

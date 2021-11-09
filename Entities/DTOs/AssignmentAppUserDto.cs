using System;
using System.Collections.Generic;

namespace Entities.DTOs
{
    public class AssignmentAppUserDto
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }
        public AppUserDTO AppUser { get; set; }

        public int AssignmentId { get; set; }
        public AssignmentDTO Assignment { get; set; }

        public bool IsSubmitted { get; set; }

        public decimal Grade { get; set; } = 0;

        public DateTime? SubmissionDate { get; set; }

        public List<AssignmentAppUserMaterialDTO> AssignmentAppUserMaterials { get; set; }
    }
}

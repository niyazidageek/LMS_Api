using System;
using System.Collections.Generic;
using Core.Entities;

namespace Entities.Models
{
    public class AssignmentAppUser:IEntity
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }

        public bool IsSubmitted { get; set; }

        public decimal Grade { get; set; } = 0;

        public bool Graded { get; set; }

        public DateTime? SubmissionDate { get; set; }

        public bool isLate { get; set; }

        public List<AssignmentAppUserMaterial> AssignmentAppUserMaterials { get; set; }
    }
}

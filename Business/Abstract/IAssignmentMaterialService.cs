using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IAssignmentMaterialService
    {
        Task<List<AssignmentMaterial>> GetAssignmentMaterialsByAssignmentIdAsync(int assignmentId);

        Task<bool> CreateAssignmentMaterialsAsync(List<AssignmentMaterial> assignmentMaterials);

        Task<bool> DeleteAssignmentMaterialsAsync(List<AssignmentMaterial> assignmentMaterials);
    }
}

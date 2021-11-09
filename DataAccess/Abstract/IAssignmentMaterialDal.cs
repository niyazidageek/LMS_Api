using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IAssignmentMaterialDal: IRepository<AssignmentMaterial>
    {
        Task<List<AssignmentMaterial>> GetAllByAssignmentIdAsync(int assignmentId);

        Task<bool> CreateAssignmentMaterialsAsync(List<AssignmentMaterial> assignmentMaterials);

        Task<bool> DeleteAssignmentMaterialssAsync(List<AssignmentMaterial> assignmentMaterials);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IAssignmentAppUserMaterialService
    {
        Task<bool> CreateAssignmentAppUserMaterialAsync(List<AssignmentAppUserMaterial> assignmentAppUserMaterials);
    }
}

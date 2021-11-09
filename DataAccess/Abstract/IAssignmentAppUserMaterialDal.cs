using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IAssignmentAppUserMaterialDal: IRepository<AssignmentAppUserMaterial>
    {
        Task<bool> CreateAssignmentAppUserMaterialAsync(List<AssignmentAppUserMaterial> assignmentAppUserMaterials);
    }
}

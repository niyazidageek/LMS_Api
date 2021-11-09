using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class AssignmentMaterialService: IAssignmentMaterialService
    {
        private readonly IAssignmentAppUserMaterialDal _conext;

        public AssignmentMaterialService(IAssignmentAppUserMaterialDal context)
        {
            _conext = context;
        }

        public async Task<List<AssignmentMaterial>> GetAssignmentMaterialsByAssignmentIdAsync(int assignmentId)
        {
            return await _conext.GetAllByAssignmentIdAsync(assignmentId);
        }

        public async Task<bool> CreateAssignmentMaterialsAsync(List<AssignmentMaterial> assignmentMaterials)
        {
            await _conext.CreateAssignmentMaterialsAsync(assignmentMaterials);

            return true;
        }

        
        public async Task<bool> DeleteAssignmentMaterialsAsync(List<AssignmentMaterial> assignmentMaterials)
        {
            await _conext.DeleteAssignmentMaterialssAsync(assignmentMaterials);

            return true;
        }
    }
}

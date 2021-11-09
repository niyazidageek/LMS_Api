using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class AssignmentAppUserMaterialService:IAssignmentAppUserMaterialService
    {
        private readonly IAssignmentAppUserMaterialDal _context;

        public AssignmentAppUserMaterialService(IAssignmentAppUserMaterialDal context)
        {
            _context = context;
        }

        public async Task<bool> CreateAssignmentAppUserMaterialAsync(List<AssignmentAppUserMaterial> assignmentAppUserMaterials)
        {
            await _context.CreateAssignmentAppUserMaterialAsync(assignmentAppUserMaterials);

            return true;
        }
    }
}

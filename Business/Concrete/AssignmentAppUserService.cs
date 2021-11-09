using System;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class AssignmentAppUserService:IAssignmentAppUserService
    {
        private readonly IAssignmentAppUserDal _context;

        public AssignmentAppUserService(IAssignmentAppUserDal context)
        {
            _context = context;
        }

        public async Task<bool> InitializeAssignmentAsync(Lesson lesson, int assignmentId)
        {
            await _context.InitializeAssignmentAsync(lesson, assignmentId);

            return true;
        }
    }
}

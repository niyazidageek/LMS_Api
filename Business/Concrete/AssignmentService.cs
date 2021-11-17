using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class AssignmentService:IAssignmentService
    {
        private readonly IAssignmentDal _context;

        public AssignmentService(IAssignmentDal context)
        {
            _context = context;
        }

        public async Task<bool> AddAssignmentAsync(Assignment assignment)
        {
            await _context.AddAsync(assignment);

            return true;
        }

        public async Task<bool> DeleteAssignmentAsync(int id)
        {
            await _context.DeleteAsync(new Assignment { Id = id });

            return true;
        }

        public async Task<bool> EditAssignmentAsync(Assignment assignment)
        {
            await _context.UpdateAsync(assignment);

            return true;
        }

        public async Task<Assignment> GetAssignmentByIdAsync(int id)
        {
            return await _context.GetByIdAsync(id);
        }

        public async Task<List<Assignment>> GetAssignmentsByLessonIdAsync(int lessonId)
        {
            return await _context.GetAllByLessonIdAsync(lessonId);
        }

        public async Task<List<Assignment>> GetAssignmentsByLessonIdAndUserIdAsync(int lessonId, string userId)
        {
            return await _context.GetAssignmentsByLessonIdAndUserIdAsync(lessonId, userId);
        }

        public async Task<Assignment> GetAssignmentByIdAndUserIdAsync(int assignmentId, string userId)
        {
            return await _context.GetByIdAndUserIdAsync(assignmentId, userId);
        }

        public async Task<List<Assignment>> GetAllByGroupIdAsync(int groupId)
        {
            return await _context.GetAllByGroupIdAsync(groupId);
        }
    }
}

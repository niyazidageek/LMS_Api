using System;
using System.Collections.Generic;
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

        public async Task<bool> EditAssignmentAppUserAsync(AssignmentAppUser assignmentAppUser)
        {
            await _context.UpdateAsync(assignmentAppUser);

            return true;
        }

        public async Task<AssignmentAppUser> GetAssignmentAppUserByIdAsync(int id)
        {
            return await _context.GetAsync(aa => aa.Id == id);
        }

        public async Task<List<AssignmentAppUser>> GetAssignmentAppUsersByLessonIdAndUserIdAsync(int lessonId, string appUserId)
        {
            return await _context.GetAssignmentAppUsersByLessonIdAndUserIdAsync(lessonId, appUserId);
        }

        public async Task<bool> InitializeAssignmentAsync(Lesson lesson, int assignmentId)
        {
            await _context.InitializeAssignmentAsync(lesson, assignmentId);

            return true;
        }
    }
}

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

        public async Task<List<int?>> GetAllPossibleSubmissionsCountByGroupIdAndYearAsync(int monthsCount, int groupId, int year)
        {
            return await _context.GetAllPossibleSubmissionsCountByGroupIdAndYearAsync(monthsCount, groupId, year);
        }

        public async Task<List<int>> GetAllSubmissionsCountByGroupIdAndYearAsync(int monthsCount, int groupId, int year)
        {
            return await _context.GetAllSubmissionsCountByGroupIdAndYearAsync(monthsCount, groupId, year);
        }

        public async Task<AssignmentAppUser> GetAssignmentAppUserByAssignmentIdAndUserIdAsync(int assignmentId, string userId)
        {
            return await _context.GetAssignmentAppUserByAssignmentIdAndUserIdAsync(assignmentId, userId);
        }

        public async Task<AssignmentAppUser> GetAssignmentAppUserByIdAsync(int id)
        {
            return await _context.GetAsync(id);
        }

        public async Task<List<AssignmentAppUser>> GetAssignmentAppUsersByAppUserIdAndGroupIdAsync(string userId, int groupId)
        {
            return await _context.GetAssignmentAppUsersByAppUserIdAndGroupIdAsync(userId, groupId);
        }

        public async Task<List<AssignmentAppUser>> GetAssignmentAppUsersByLessonIdAsync(int lessonId)
        {
            return await _context.GetAssignmentAppUsersByLessonIdAsync(lessonId);
        }

        public async Task<int> GetAssignmentAppUsersByLessonIdCountAsync(int lessonId)
        {
            return await _context.GetAssignmentAppUsersByLessonIdCountAsync(lessonId);
        }

        public async Task<bool> InitializeAssignmentAsync(List<AppUserGroup> appUserGroups, int assignmentId)
        {
            await _context.InitializeAssignmentAsync(appUserGroups, assignmentId);

            return true;
        }

        public async Task<bool> ReinitializeAssignmentsAsync(List<AppUserGroup> appUserGroups, List<Assignment> assignments)
        {
            return await _context.ReinitializeAssignmentsAsync(appUserGroups, assignments);
        }
    }
}

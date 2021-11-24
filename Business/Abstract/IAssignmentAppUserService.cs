using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IAssignmentAppUserService
    {
        Task<List<AssignmentAppUser>> GetAssignmentAppUsersByLessonIdAsync(int lessonId);

        Task<AssignmentAppUser> GetAssignmentAppUserByIdAsync(int id);

        Task<bool> InitializeAssignmentAsync(List<AppUserGroup> appUserGroups, int assignmentId);

        Task<bool> EditAssignmentAppUserAsync(AssignmentAppUser assignmentAppUser);

        Task<AssignmentAppUser> GetAssignmentAppUserByAssignmentIdAndUserIdAsync(int assignmentId, string userId);

        Task<List<AssignmentAppUser>> GetAssignmentAppUsersByAppUserIdAndGroupIdAsync(string userId, int groupId);

        Task<bool> ReinitializeAssignmentsAsync(List<AppUserGroup> appUserGroups, List<Assignment> assignments);

        Task<List<int>> GetAllSubmissionsCountByGroupIdAndYearAsync(int monthsCount, int groupId, int year);

        Task<int> GetAssignmentAppUsersByLessonIdCountAsync(int lessonId);

        Task<List<int?>> GetAllPossibleSubmissionsCountByGroupIdAndYearAsync(int monthsCount, int groupId, int year);
    }
}

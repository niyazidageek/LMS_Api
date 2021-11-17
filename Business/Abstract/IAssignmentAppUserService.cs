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
    }
}

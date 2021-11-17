using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IAssignmentAppUserDal: IRepository<AssignmentAppUser>
    {
        Task<bool> InitializeAssignmentAsync(List<AppUserGroup> appUserGroups, int assignmentId);

        Task<List<AssignmentAppUser>> GetAssignmentAppUsersByLessonIdAsync(int lessonId);

        Task<AssignmentAppUser> GetAsync(int id);

        Task<AssignmentAppUser> GetAssignmentAppUserByAssignmentIdAndUserIdAsync(int assignmentId, string userId);

        Task<List<AssignmentAppUser>> GetAssignmentAppUsersByAppUserIdAndGroupIdAsync(string userId, int groupId);

        Task<bool> ReinitializeAssignmentsAsync(List<AppUserGroup> appUserGroups, List<Assignment> assignments);
    }
}

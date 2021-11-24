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

        Task<int> GetAssignmentAppUsersByLessonIdCountAsync(int lessonId);

        Task<List<int>> GetAllSubmissionsCountByGroupIdAndYearAsync(int monthsCount, int groupId, int year);

        Task<AssignmentAppUser> GetAssignmentAppUserByAssignmentIdAndUserIdAsync(int assignmentId, string userId);

        Task<List<AssignmentAppUser>> GetAssignmentAppUsersByAppUserIdAndGroupIdAsync(string userId, int groupId);

        Task<bool> ReinitializeAssignmentsAsync(List<AppUserGroup> appUserGroups, List<Assignment> assignments);

        Task<List<int?>> GetAllPossibleSubmissionsCountByGroupIdAndYearAsync(int monthsCount, int groupId, int year);

        Task<List<int>> GetPossibleYearsAsync(int groupId);
    }
}

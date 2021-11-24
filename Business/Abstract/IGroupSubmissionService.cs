using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IGroupSubmissionService
    {
        Task<List<GroupSubmission>> GetGroupSubmissionsAsync();

        Task<GroupSubmission> GetGroupSubmissionByIdAsync(int id);

        Task<bool> AddGroupSubmissionAsync(GroupSubmission groupSubmission);

        Task<bool> EditGroupSubmissionAsync(GroupSubmission groupSubmission);

        Task<bool> DeleteGroupSubmissionAsync(int id);

        Task<List<GroupSubmission>> GetAllGroupSubmissionsByGroupIdAndYearAsync(int groupId, int year);

        Task<List<int>> GetPossibleYearsAsync(int groupId);
    }
}

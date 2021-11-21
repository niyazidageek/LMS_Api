using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IAssignmentService
    {
        Task<List<Assignment>> GetAssignmentsByLessonIdAsync(int lessonId);

        Task<Assignment> GetAssignmentByIdAsync(int id);

        Task<bool> AddAssignmentAsync(Assignment assignment);

        Task<bool> EditAssignmentAsync(Assignment assignment);

        Task<bool> DeleteAssignmentAsync(int id);

        Task<List<Assignment>> GetAssignmentsByGroupIdAsync(int groupId);

        Task<List<Assignment>> GetAssignmentsByLessonIdAndUserIdAsync(int lessonId, string userId);

        Task<Assignment> GetAssignmentByIdAndUserIdAsync(int assignmentId, string userId);

        Task<int> GetAssignmentsByGroupIdCountAsync(int groupId);

        Task<List<Assignment>> GetAssignmentsByGroupIdAsync(int groupId, int page = 0, int size = 3);
    }
}

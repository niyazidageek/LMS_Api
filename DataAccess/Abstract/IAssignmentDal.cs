using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IAssignmentDal:IRepository<Assignment>
    {
        Task<List<Assignment>> GetAllByLessonIdAsync(int lessonId);

        Task<List<Assignment>> GetAssignmentsByLessonIdAndUserIdAsync(int lessonId, string appUserId);

        Task<Assignment> GetByIdAsync(int assignmentId);

        Task<Assignment> GetByIdAndUserIdAsync(int assignmentId, string userId);

        Task<List<Assignment>> GetAllByGroupIdAsync(int groupId);

        Task<int> GetAssignmentsByGroupIdCountAsync(int groupId);

        Task<List<Assignment>> GetAllByGroupIdAsync(int groupId, int page = 0, int size = 3);
    }
}

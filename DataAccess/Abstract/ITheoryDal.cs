using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface ITheoryDal: IRepository<Theory>
    {
        Task<List<Theory>> GetTheoriesByLessonIdAndUserIdAsync(int lessonId, string appUserId);

        Task<List<Theory>> GetAllByLessonIdAsync(int lessonId);

        Task<Theory> GetByIdAndUserId(int theoryId, string userId);

        Task<List<Theory>> GetAllByGroupIdAsync(int groupId);

        Task<List<Theory>> GetAllByGroupIdAsync(int groupId, int page = 0, int size = 3);

        Task<int> GetTheoriesByGroupIdCountAsync(int groupId);

        Task<Theory> GetAsync(int id);
    }
}

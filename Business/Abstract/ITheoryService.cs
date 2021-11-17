using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface ITheoryService
    {
        Task<List<Theory>> GetTheoriesByLessonIdAsync(int lessonId);

        Task<Theory> GetTheoryByIdAsync(int id);

        Task<bool> AddTheoryAsync(Theory theory);

        Task<bool> EditTheoryAsync(Theory theory);

        Task<bool> DeleteTheoryAsync(int id);

        Task<List<Theory>> GetTheoriesByLessonIdAndUserIdAsync(int lessonId, string appUserId);

        Task<Theory> GetTheoryByIdAndUserId(int theoryId, string userId);

        Task<List<Theory>> GetAllByGroupIdAsync(int groupId);
    }
}

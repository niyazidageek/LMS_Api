using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IQuizDal: IRepository<Quiz>
    {
        Task<bool> DeleteWithQuestionsAndOptionsAsync(int id);

        Task<List<Quiz>> GetAllAsync();

        Task<Quiz> GetAsync(int id);

        Task<List<Quiz>> GetAllByGroupIdAsync(int groupId);

        Task<int> GetQuizzesCountByGroupIdAsync(int groupId);

        Task<List<Quiz>> GetAllByGroupIdAsync(int groupId, int page = 0, int size = 3);

        Task<Quiz> GetByUserIdAsync(int id, string userId);

        Task<List<Quiz>> GetAllByGroupIdAndUserIdAsync(string userId, int groupId, int page = 0, int size = 3);

        Task<int> GetQuizzesCountByGroupIdAndUserIdAsync(string userId, int groupId);

        Task<Quiz> GetInfoByUserIdAsync(int id, string userId);
    }
}

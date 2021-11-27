using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IQuizService
    {
        Task<List<Quiz>> GetQuizzesAsync();

        Task<Quiz> GetQuizByIdAsync(int id);

        Task<bool> AddQuizAsync(Quiz quiz);

        Task<bool> EditQuizAsync(Quiz quiz);

        Task<bool> DeleteQuizAsync(int id);

        Task<bool> DeleteQuizWithQuestionsAndOptionsAsync(int id);

        Task<List<Quiz>> GetQuizzesByGroupIdAsync(int groupId);

        Task<int> GetQuizzesCountByGroupIdAsync(int groupId);

        Task<List<Quiz>> GetQuizzesByGroupIdAsync(int groupId, int page = 0, int size = 3);

        Task<Quiz> GetQuizByUserIdAsync(int id, string userId);

        Task<List<Quiz>> GetQuizzesByGroupIdAndUserIdAsync(string userId, int groupId, int page = 0, int size = 3);

        Task<int> GetQuizzesCountByGroupIdAndUserIdAsync(string userId, int groupId);

        Task<Quiz> GetQuizInfoByUserIdAsync(int id, string userId);
    }
}

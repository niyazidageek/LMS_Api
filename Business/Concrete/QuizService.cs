using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class QuizService:IQuizService
    {
        private readonly IQuizDal _context;

        public QuizService(IQuizDal context)
        {
            _context = context;
        }

        public async Task<bool> AddQuizAsync(Quiz quiz)
        {
            await _context.AddAsync(quiz);

            return true;
        }

        public async Task<bool> DeleteQuizAsync(int id)
        {
            await _context.DeleteAsync(new Quiz { Id = id });

            return true;
        }

        public async Task<bool> DeleteQuizWithQuestionsAndOptionsAsync(int id)
        {
            await _context.DeleteWithQuestionsAndOptionsAsync(id);

            return true;
        }

        public async Task<bool> EditQuizAsync(Quiz quiz)
        {
            await _context.UpdateAsync(quiz);

            return true;
        }

        public async Task<List<Quiz>> GetAllByGroupIdAsync(int groupId)
        {
            return await _context.GetAllByGroupIdAsync(groupId);
        }

        public async Task<Quiz> GetQuizByIdAsync(int id)
        {
            return await _context.GetAsync(id);
        }

        public async Task<Quiz> GetQuizByUserIdAsync(int id, string userId)
        {
            return await _context.GetByUserIdAsync(id, userId);
        }

        public async Task<Quiz> GetQuizInfoByUserIdAsync(int id, string userId)
        {
            return await _context.GetInfoByUserIdAsync(id, userId);
        }

        public async Task<List<Quiz>> GetQuizzesAsync()
        {
            return await _context.GetAllAsync();
        }

        public async Task<List<Quiz>> GetQuizzesByGroupIdAndUserIdAsync(string userId, int groupId, int page = 0, int size = 3)
        {
            return await _context.GetAllByGroupIdAndUserIdAsync(userId, groupId, page, size);
        }

        public async Task<List<Quiz>> GetQuizzesByGroupIdAsync(int groupId)
        {
            return await _context.GetAllByGroupIdAsync(groupId);
        }

        public async Task<List<Quiz>> GetQuizzesByGroupIdAsync(int groupId, int page = 0, int size = 3)
        {
            return await _context.GetAllByGroupIdAsync(groupId, page, size);
        }

        public async Task<int> GetQuizzesCountByGroupIdAndUserIdAsync(string userId, int groupId)
        {
            return await _context.GetQuizzesCountByGroupIdAndUserIdAsync(userId, groupId);
        }

        public async Task<int> GetQuizzesCountByGroupIdAsync(int groupId)
        {
            return await _context.GetQuizzesCountByGroupIdAsync(groupId);
        }
    }
}

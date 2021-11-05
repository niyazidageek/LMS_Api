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

        public async Task<Quiz> GetQuizByIdAsync(int id)
        {
            return await _context.GetAsync(id);
        }

        public async Task<List<Quiz>> GetQuizzesAsync()
        {
            return await _context.GetAllAsync();
        }
    }
}

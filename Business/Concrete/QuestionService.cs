using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionDal _context;

        public QuestionService(IQuestionDal context)
        {
            _context = context;
        }

        public async Task<bool> AddQuestionWithFileAsync(Question question)
        {
            await _context.AddWithFileAsync(question);

            return true;
        }

        public async Task<bool> AddQuestionAsync(Question question)
        {
            await _context.AddAsync(question);

            return true;
        }

        public async Task<bool> DeleteQuestionAsync(Question question)
        {
            await _context.DeleteAsync(question);

            return true;
        }

        public async Task<bool> EditQuestionWithFileAsync(Question question)
        {
            await _context.UpdateWithFileAsync(question);

            return true;
        }

        public async Task<bool> EditQuestionWithoutFileAsync(Question question)
        {
            await _context.UpdateWithoutFileAsync(question);

            return true;
        }

        public async Task<Question> GetQuestionByIdAsync(int id)
        {
            return await _context.GetAsync(id);
        }

        public async Task<bool> EditQuestionAsync(Question question)
        {
            await _context.UpdateAsync(question);

            return true;
        }

        public async Task<int> GetQuestionsCountByQuizIdAsync(int quizId)
        {
            return await _context.GetQuestionsCountByQuizIdAsync(quizId);
        }

        public async Task<Question> GetQuestionByPageAndQuizId(int page, int quizId)
        {
            return await _context.GetQuestionByPageAndQuizId(page, quizId);
        }

        public async Task<List<Question>> GetQuestionsByQuizId(int quizId)
        {
            return await _context.GetAllByQuizId(quizId);
        }

        public async Task<List<Question>> GetQuestionsByQuizId(int quizId, int page = 0, int take = 3)
        {
            return await _context.GetAllByQuizId(quizId, page, take);
        }
    }
}

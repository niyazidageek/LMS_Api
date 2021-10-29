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
            await _context.DeleteWithOptionsAsync(question);

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
            return await _context.GetQuestionWithOptionsAsync(id);
        }

        public async Task<List<Question>> GetQuestionsAsync()
        {
            return await _context.GetQuestionsWithOptionsAsync();
        }

        public async Task<bool> EditQuestionAsync(Question question)
        {
            await _context.UpdateAsync(question);

            return true;
        }
    }
}

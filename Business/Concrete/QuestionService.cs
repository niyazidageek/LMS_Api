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

        public async Task<bool> AddQuestionWihtFileAsync(Question question)
        {
            await _context.AddWithFileAsync(question);

            return true;
        }

        public async Task<bool> AddQuestionAsync(Question question)
        {
            await _context.AddAsync(question);

            return true;
        }

        public async Task<bool> DeleteQuestionAsync(int id)
        {
            await _context.DeleteWithOptionsAsync(new Question { Id = id });

            return true;
        }

        public async Task<bool> EditQuestionWithFileAsync(Question question)
        {
            await _context.UpdateAsync(question);

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

        public Task<bool> AddQuestionWithFileAsync(Question question)
        {
            throw new NotImplementedException();
        }
    }
}

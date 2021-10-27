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

        public async Task<bool> AddQuestionAsync(Question question, List<Option> options)
        {
            await _context.AddAsync(question, options);

            return true;
        }

        public async Task<bool> DeleteQuestionAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EditQuestionAsync(Question question, List<Option> options)
        {
            await _context.UpdateAsync(question, options);

            return true;
        }

        public async Task<Question> GetQuestionByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Question>> GetQuestionsAsync()
        {
            throw new NotImplementedException();
        }
    }
}

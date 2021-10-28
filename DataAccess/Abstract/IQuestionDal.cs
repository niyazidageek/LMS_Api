using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IQuestionDal: IRepository<Question>
    {
        public Task<List<Question>> GetQuestionsWithOptionsAsync();

        public Task<Question> GetQuestionWithOptionsAsync(int id);

        public Task<bool> AddWithFileAsync(Question question);

        public Task<bool> UpdateWithFileAsync(Question question);

        public Task<bool> UpdateWithoutFileAsync(Question question);

        public Task<bool> DeleteWithOptionsAsync(Question question);
    }
}

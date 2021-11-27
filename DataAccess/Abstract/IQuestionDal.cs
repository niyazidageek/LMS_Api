using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IQuestionDal: IRepository<Question>
    {
        public Task<bool> AddWithFileAsync(Question question);

        public Task<bool> UpdateWithFileAsync(Question question);

        public Task<bool> UpdateWithoutFileAsync(Question question);

        public new Task<bool> DeleteAsync(Question question);

        public Task<int> GetQuestionsCountByQuizIdAsync(int quizId);

        public Task<Question> GetQuestionByPageAndQuizId(int page,int quizId);

        Task<List<Question>> GetAllByQuizId(int quizId, int page = 0, int size = 3);

        Task<List<Question>> GetAllByQuizId(int quizId);

        Task<Question> GetAsync(int id);
    }
}

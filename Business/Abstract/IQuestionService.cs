using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IQuestionService
    {
        Task<List<Question>> GetQuestionsByQuizId(int quizId);

        Task<List<Question>> GetQuestionsByQuizId(int quizId, int page=0, int take=3);

        Task<Question> GetQuestionByIdAsync(int id);

        Task<bool> AddQuestionWithFileAsync(Question question);

        Task<bool> AddQuestionAsync(Question question);

        Task<bool> EditQuestionWithFileAsync(Question question);

        Task<bool> EditQuestionWithoutFileAsync(Question question);

        Task<bool> DeleteQuestionAsync(Question question);

        Task<bool> EditQuestionAsync(Question question);

        Task<int> GetQuestionsCountByQuizIdAsync(int quizId);

        Task<Question> GetQuestionByPageAndQuizId(int page, int quizId);
    }
}

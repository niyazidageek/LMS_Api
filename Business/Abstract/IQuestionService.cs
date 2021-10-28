using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IQuestionService
    {
        Task<List<Question>> GetQuestionsAsync();

        Task<Question> GetQuestionByIdAsync(int id);

        Task<bool> AddQuestionWithFileAsync(Question question);

        Task<bool> AddQuestionAsync(Question question);

        Task<bool> EditQuestionWithFileAsync(Question question);

        Task<bool> EditQuestionWithoutFileAsync(Question question);

        Task<bool> DeleteQuestionAsync(int id);
    }
}

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

        Task<bool> AddQuestionAsync(Question question, List<Option> options);

        Task<bool> EditQuestionAsync(Question question, List<Option> options);

        Task<bool> DeleteQuestionAsync(int id);
    }
}

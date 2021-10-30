using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IQuizService
    {
        Task<List<Quiz>> GetQuizzesAsync();

        Task<Quiz> GetQuizByIdAsync(int id);

        Task<bool> AddQuizAsync(Quiz quiz);

        Task<bool> EditQuizAsync(Quiz quiz);

        Task<bool> DeleteQuizAsync(int id);

        Task<bool> DeleteQuizWithQuestionsAndOptionsAsync(int id);
    }
}

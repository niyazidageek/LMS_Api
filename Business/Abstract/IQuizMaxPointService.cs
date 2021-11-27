using System;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IQuizMaxPointService
    {
        Task<QuizMaxPoint> GetQuizMaxPointByQuizId(int quizId);

        Task<bool> EditQuizMaxPoint(QuizMaxPoint quizMaxPoint);
    }
}

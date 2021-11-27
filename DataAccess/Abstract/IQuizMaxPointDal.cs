using System;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IQuizMaxPointDal : IRepository<QuizMaxPoint>
    {
        Task<QuizMaxPoint> GetByQuizIdAsync(int quizId);
    }
}

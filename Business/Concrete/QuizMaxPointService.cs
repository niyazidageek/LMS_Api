using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class QuizMaxPointService : IQuizMaxPointService
    {
        private readonly IQuizMaxPointDal _context;

        public QuizMaxPointService(IQuizMaxPointDal context)
        {
            _context = context;
        }

        public async Task<bool> EditQuizMaxPoint(QuizMaxPoint quizMaxPoint)
        {
            await _context.UpdateAsync(quizMaxPoint);

            return true;
        }

        public async Task<QuizMaxPoint> GetQuizMaxPointByQuizId(int quizId)
        {
            return await _context.GetByQuizIdAsync(quizId);
        }
    }
}

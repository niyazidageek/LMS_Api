using System;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class EFQuizMaxPointDal : EFRepositoryBase<QuizMaxPoint, AppDbContext>, IQuizMaxPointDal
    {
        public EFQuizMaxPointDal(AppDbContext context) : base(context)
        {
        }

        public async Task<QuizMaxPoint> GetByQuizIdAsync(int quizId)
        {
            return await Context.QuizMaxPoints.AsNoTracking()
                .FirstOrDefaultAsync(qmp => qmp.QuizId == quizId);
        }
    }
}

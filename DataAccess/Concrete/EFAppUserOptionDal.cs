using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class EFAppUserOptionDal: EFRepositoryBase<AppUserOption, AppDbContext>, IAppUserOptionDal
    {
        public EFAppUserOptionDal(AppDbContext context) : base(context)
        {
        }

        public async Task<List<AppUserOption>> GetAllByQuizIdAndUserIdAsync(int quizId, string userId)
        {
            return await Context.AppUserOptions.AsNoTracking()
                .Include(ao => ao.Option)
                .Include(ao=>ao.Question)
                .Where(ao => ao.Question.QuizId == quizId && ao.AppUserId == userId)
                .ToListAsync();
        }
    }
}

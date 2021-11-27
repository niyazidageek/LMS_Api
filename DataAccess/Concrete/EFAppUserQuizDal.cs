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
    public class EFAppUserQuizDal : EFRepositoryBase<AppUserQuiz, AppDbContext>, IAppUserQuizDal
    {
        public EFAppUserQuizDal(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> InitializeQuizAsync(List<AppUserGroup> appUserGroups, int quizId)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                foreach (var appUserGroup in appUserGroups)
                {
                    AppUserQuiz appUserQuiz = new();

                    appUserQuiz.AppUserId = appUserGroup.AppUserId;
                    appUserQuiz.QuizId = quizId;
                    appUserQuiz.IsSubmitted = false;
                    appUserQuiz.SubmissionDate = null;

                    await Context.AppUserQuizzes.AddAsync(appUserQuiz);
                }

                await Context.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync();
                throw;
            }
        }

        public async Task<AppUserQuiz> GetByUserIdAndQuizIdAsync(string userId, int quizId)
        {
            return await Context.AppUserQuizzes
                .AsNoTracking()
                .FirstOrDefaultAsync(aq => aq.AppUserId == userId && aq.QuizId == quizId);
        }
    }
}

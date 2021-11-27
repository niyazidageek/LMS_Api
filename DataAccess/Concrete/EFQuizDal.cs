using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using LMS_Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class  EFQuizDal: EFRepositoryBase<Quiz, AppDbContext>, IQuizDal
    {
        public EFQuizDal(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> DeleteWithQuestionsAndOptionsAsync(int id)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                var quizDb = await Context.Quizzes
                .FirstOrDefaultAsync(q => q.Id == id);

                var questionsDb = await Context.Questions
                    .Include(q => q.Quiz)
                    .Include(q => q.Options)
                    .Where(q => q.Quiz.Id == id)
                    .ToListAsync();

                if(questionsDb is not null)
                {
                    foreach (var question in questionsDb)
                    {
                        if(question.Options is not null)
                        {
                            foreach (var option in question.Options)
                            {
                                FileHelper.DeleteFile(option.FileName);
                                Context.Options.Remove(option);
                            }
                        }

                        FileHelper.DeleteFile(question.FileName);
                        Context.Questions.Remove(question);
                    }
                }

                Context.Quizzes.Remove(quizDb);
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

        public async Task<List<Quiz>> GetAllAsync()
        {
            return await Context.Quizzes.AsNoTracking()
                .Include(q=>q.QuizMaxPoint)
                .ToListAsync();
        }

        public async Task<Quiz> GetAsync(int id)
        {
            return await Context.Quizzes.AsNoTracking()
                .Include(q => q.QuizMaxPoint)
                .Include(q => q.Group)
                .ThenInclude(q=>q.AppUserGroups)
                .Include(q=>q.Questions)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<List<Quiz>> GetAllByGroupIdAsync(int groupId)
        {
            return await Context.Quizzes
                .AsNoTracking()
                .Include(q => q.QuizMaxPoint)
                .Where(q=>q.GroupId == groupId)
                .ToListAsync();
        }

        public async Task<int> GetQuizzesCountByGroupIdAsync(int groupId)
        {
            return await Context.Quizzes
                .AsNoTracking().CountAsync(q => q.GroupId == groupId);
        }

        public async Task<Quiz> GetByUserIdAsync(int id, string userId)
        {
            return await Context.Quizzes.AsNoTracking()
                .Include(q => q.QuizMaxPoint)
                .Include(q => q.Group)
                .Include(q => q.Questions)
                .Include(q=>q.AppUserQuizzes.Where(aq=>aq.AppUserId == userId))
                .FirstOrDefaultAsync(q => q.Id == id && q.AppUserQuizzes.Any(aq=>aq.AppUserId == userId));
        }

        public async Task<Quiz> GetInfoByUserIdAsync(int id, string userId)
        {
            return await Context.Quizzes.AsNoTracking()
                .Include(q => q.QuizMaxPoint)
                .Include(q => q.AppUserQuizzes.Where(aq => aq.AppUserId == userId))
                .FirstOrDefaultAsync(q => q.Id == id && q.AppUserQuizzes.Any(aq => aq.AppUserId == userId));
        }

        public async Task<List<Quiz>> GetAllByGroupIdAsync(int groupId, int page = 0, int size = 3)
        {
            return await Context.Quizzes
                .AsNoTracking()
                .Include(q => q.QuizMaxPoint)
                .Where(q => q.GroupId == groupId)
                .OrderByDescending(q=>q.Deadline)
                .Skip(page * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<List<Quiz>> GetAllByGroupIdAndUserIdAsync(string userId,int groupId, int page = 0, int size = 3)
        {
            return await Context.Quizzes
                .AsNoTracking()
                .Include(q => q.QuizMaxPoint)
                .Include(q=>q.AppUserQuizzes.Where(aq=>aq.AppUserId == userId))
                .Where(q => q.GroupId == groupId && q.IsAvailable == true && q.AppUserQuizzes.Any(q=>q.AppUserId == userId))
                .OrderByDescending(q => q.Deadline)
                .Skip(page * size)
                .Take(size)
                .ToListAsync();
        }


        public async Task<int> GetQuizzesCountByGroupIdAndUserIdAsync(string userId, int groupId)
        {
            return await Context.Quizzes
                .AsNoTracking()
                .Include(q => q.QuizMaxPoint)
                .Include(q => q.AppUserQuizzes)
                .CountAsync(q => q.GroupId == groupId && q.IsAvailable == true && q.AppUserQuizzes.Any(q => q.AppUserId == userId));
        }
    }
}

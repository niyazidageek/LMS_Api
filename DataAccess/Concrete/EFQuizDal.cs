using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using LMS_Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class EFQuizDal: EFRepositoryBase<Quiz, AppDbContext>, IQuizDal
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
    }
}

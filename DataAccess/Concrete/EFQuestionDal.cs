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
    public class EFQuestionDal : EFRepositoryBase<Question, AppDbContext>, IQuestionDal
    {
        public EFQuestionDal(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> AddWithFileAsync(Question question)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                var fileName = FileHelper.AddFile(question.File);

                question.FileName = fileName;
               
                await Context.Questions.AddAsync(question);
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

        public async Task<bool> DeleteWithOptionsAsync(Question question)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                var optionsDb = await Context.Options
                    .Where(o => o.Question.Id == question.Id)
                    .ToListAsync();

                if (optionsDb is not null)
                {
                    foreach (var optionDb in optionsDb)
                    {
                        
                        if(optionDb.FileName is not null)
                        {
                            FileHelper.DeleteFile(optionDb.FileName);
                        }
                        Context.Options.Remove(optionDb);
                    }
                }


                if (question.FileName is not null)
                {
                    FileHelper.DeleteFile(question.FileName);
                }

                Context.Questions.Remove(question);
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

        public async Task<List<Question>> GetQuestionsWithOptionsAsync()
        {
            return await Context.Questions
                .Include(q => q.Options)
                .ToListAsync();
        }

        public async Task<Question> GetQuestionWithOptionsAsync(int id)
        {
            return await Context.Questions
                .AsNoTracking()
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<bool> UpdateWithFileAsync(Question question)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {

                if (question.FileName is not null)
                {           
                    FileHelper.DeleteFile(question.FileName);
                }

                var fileName = FileHelper.AddFile(question.File);

                question.FileName = fileName;
              
                Context.Questions.Update(question);
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

        public async Task<bool> UpdateWithoutFileAsync(Question question)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                FileHelper.DeleteFile(question.FileName);
                question.FileName = null;

                Context.Questions.Update(question);
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

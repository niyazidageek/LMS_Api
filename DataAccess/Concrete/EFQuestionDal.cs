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
                var fileName = await FileHelper.AddFile(question.File);
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

        public new async Task<bool> DeleteAsync(Question question)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {

                if (question.Options is not null)
                {
                    foreach (var optionDb in question.Options)
                    {

                        if (optionDb.FileName is not null)
                        {
                            FileHelper.DeleteFile(optionDb.FileName);
                        }
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

        public async Task<Question> GetQuestionByPageAndQuizId(int page, int quizId)
        {
            return await Context.Questions
                .AsNoTracking()
                .Where(q => q.QuizId == quizId)
                .Include(q=>q.Options)
                .OrderBy(q => q.Id)
                .Skip(page)
                .Take(1)
                .FirstAsync();
        }

        public async Task<int> GetQuestionsCountByQuizIdAsync(int quizId)
        {
            return await Context.Questions
                .AsNoTracking().CountAsync(q => q.QuizId == quizId);
        }

        public async Task<List<Question>> GetAllByQuizId(int quizId, int page = 0, int size = 3)
        {
            return await Context.Questions
                .AsNoTracking()
                .Where(q => q.QuizId == quizId)
                .Include(q => q.Options)
                .OrderByDescending(q=>q.Id)
                .Skip(page*size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<List<Question>> GetAllByQuizId(int quizId)
        {
            return await Context.Questions
                .AsNoTracking()
                .Where(q => q.QuizId == quizId)
                .ToListAsync();
        }

        public async Task<Question> GetAsync(int id)
        {
            return await Context.Questions
                .AsNoTracking()
                .Include(q => q.Quiz)
                .Include(q=>q.Options)
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

                var fileName = await FileHelper.AddFile(question.File);

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
                var existingQuestion = await Context.Questions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(q => q.Id == question.Id);


                if(question.FileName is null)
                {
                    if (existingQuestion.FileName is not null)
                    {
                        FileHelper.DeleteFile(existingQuestion.FileName);
                    }
                }

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

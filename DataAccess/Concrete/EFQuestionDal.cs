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
                var materialDb = new Material();

                var fileName = FileHelper.AddFile(question.File);
                materialDb.FileName = fileName;

                await Context.Materials.AddAsync(materialDb);

                question.Material = materialDb;
               
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
                var optionsDb = await Context.Options.Where(o => o.Question.Id == question.Id)
                    .Include(o=>o.Material)
                    .ToListAsync();

                if (optionsDb is not null)
                {
                    foreach (var optionDb in optionsDb)
                    {
                        //var fileDb = await Context.Materials
                        //    .FirstOrDefaultAsync(m=>m.FileName == optionDb.Material.FileName);
                        Context.Options.Remove(optionDb);
                    }
                }

                var questionDb = await Context.Questions.Include(q => q.Material)
                    .FirstOrDefaultAsync(q=>q.Id == question.Id);

                Context.Questions.Remove(questionDb);
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
            return await Context.Questions.Include(q => q.Material)
                .Include(q => q.Options)
                .ThenInclude(o => o.Material)
                .ToListAsync();
        }

        public async Task<Question> GetQuestionWithOptionsAsync(int id)
        {
            return await Context.Questions.Include(q => q.Material)
                .Include(q => q.Options)
                .ThenInclude(o => o.Material)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<bool> UpdateWithFileAsync(Question question)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                var materialDb = new Material();

                var fileName = FileHelper.AddFile(question.File);
                materialDb.FileName = fileName;


                if (question.Material is not null)
                {
                    var existingMaterialDb = await Context.Materials
                    .FirstOrDefaultAsync(m => m.FileName == question.Material.FileName);

                    Context.Materials.Remove(existingMaterialDb);
                    FileHelper.DeleteFile(existingMaterialDb.FileName);
                }

                await Context.Materials.AddAsync(materialDb);

                question.Material = materialDb;
              
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
                
                var existingMaterialDb = await Context.Materials
                            .FirstOrDefaultAsync(m => m.FileName == question.Material.FileName);

                Context.Materials.Remove(existingMaterialDb);
                await Context.SaveChangesAsync();
                FileHelper.DeleteFile(existingMaterialDb.FileName);

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

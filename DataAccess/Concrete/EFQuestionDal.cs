using System;
using System.Collections.Generic;
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

        public async Task<bool> AddAsync(Question question, List<Option> options)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                if(question.File is not null)
                {
                    var materialDb = new Material();

                    var fileName = FileHelper.AddFile(question.File);
                    materialDb.FileName = fileName;

                    await Context.Materials.AddAsync(materialDb);

                    question.Material = materialDb;
                }

                foreach (var option in options)
                {
                    var materialDb = new Material();

                    var fileName = FileHelper.AddFile(option.File);
                    materialDb.FileName = fileName;

                    await Context.Materials.AddAsync(materialDb);

                    option.Material = materialDb;

                    await Context.Options.AddAsync(option);
                }

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

        public async Task<bool> UpdateAsync(Question question, List<Option> options)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {

                if(question.File is not null)
                {
                    var materialDb = new Material();
                    var fileName = FileHelper.AddFile(question.File);
                    materialDb.FileName = fileName;


                    Context.Materials.Remove(question.Material);
                    await Context.Materials.AddAsync(materialDb);

                    question.Material = materialDb;
                }

                if(question.ExistingFileName is null)
                {
                    var materialDb = await Context.Materials
                        .FirstOrDefaultAsync(m => m.Id == question.Material.Id);

                    if(materialDb is not null)
                    {
                        FileHelper.DeleteFile(materialDb.FileName);
                        Context.Materials.Remove(materialDb);
                    }
                }

                foreach (var option in options)
                {
                    if(option.File is not null)
                    {
                        var materialDb = new Material();
                        var fileName = FileHelper.AddFile(option.File);
                        materialDb.FileName = fileName;

                        Context.Materials.Remove(option.Material);
                        await Context.Materials.AddAsync(materialDb);
                    }

                    if(option.ExistingFileName is null)
                    {
                        var materialDb = await Context.Materials
                            .FirstOrDefaultAsync(m => m.Id == option.Material.Id);

                        if(materialDb is not null)
                        {
                            FileHelper.DeleteFile(materialDb.FileName);
                            Context.Materials.Remove(materialDb);
                        }
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

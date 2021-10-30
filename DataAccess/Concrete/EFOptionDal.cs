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
    public class EFOptionDal: EFRepositoryBase<Option, AppDbContext>, IOptionDal
    {
        public EFOptionDal(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> AddWithFileAsync(Option option)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                var fileName = FileHelper.AddFile(option.File);
                option.FileName = fileName;

                await Context.Options.AddAsync(option);
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

        public async Task<bool> DeleteWithFileAsync(Option option)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {

                FileHelper.DeleteFile(option.FileName);

                Context.Options.Remove(option);
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

        public async Task<List<Option>> GetOptionsByQuestion(int id)
        {
            return await Context.Options.Where(o => o.Question.Id == id)
                .ToListAsync();
        }

        public async Task<bool> UpdateWithFileAsync(Option option)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {

                if (option.FileName is not null)
                {
                    FileHelper.DeleteFile(option.FileName);
                }

                var fileName = FileHelper.AddFile(option.File);

                option.FileName = fileName;

                
                Context.Options.Update(option);
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

        public async Task<bool> UpdateWithoutFileAsync(Option option)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                FileHelper.DeleteFile(option.FileName);
                option.FileName = null;

                Context.Options.Update(option);
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

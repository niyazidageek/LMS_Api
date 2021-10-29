using System;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using LMS_Api.Utils;

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
    }
}

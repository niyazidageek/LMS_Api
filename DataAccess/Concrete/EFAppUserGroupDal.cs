using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class EFAppUserGroupDal: EFRepositoryBase<AppUserGroup, AppDbContext>, IAppUserGroupDal
    {
        public EFAppUserGroupDal(AppDbContext context):base(context)
        {
        }

        public async Task<bool> CreateAppUserGroupsAsync(List<AppUserGroup> appUserGroups)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                foreach (var appUserGroup in appUserGroups)
                {
                    await Context.AppUserGroups.AddAsync(appUserGroup);
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

        public async Task<bool> DeleteAppUserGroupsAsync(List<AppUserGroup> appUserGroups)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                foreach (var appUserGroup in appUserGroups)
                {
                    Context.AppUserGroups.Remove(appUserGroup);
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

        public async Task<AppUserGroup> GetByAppUserIdAndGroupIdAsync(string userId, int groupId)
        {
            return await Context.AppUserGroups.AsNoTracking()
                .FirstOrDefaultAsync(ag => ag.AppUserId == userId && ag.GroupId == groupId);
        }
    }
}

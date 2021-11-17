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
    public class EFTheoryAppUserDal: EFRepositoryBase<TheoryAppUser, AppDbContext>, ITheoryAppUserDal
    {
        public EFTheoryAppUserDal(AppDbContext context):base(context)
        {
        }

        public async Task<TheoryAppUser> GetTheoryAppUserByTheoryIdAndUserIdAsync(int theoryId, string userId)
        {
            return await Context.TheoryAppUsers.AsNoTracking()
               .FirstOrDefaultAsync(ta => ta.AppUserId == userId && ta.TheoryId == theoryId);
        }

        public async Task<List<TheoryAppUser>> GetTheoryAppUsersByAppUserIdAndGroupIdAsync(string userId, int groupId)
        {
            return await Context.TheoryAppUsers.AsNoTracking()
                .Include(ta => ta.Theory)
                .ThenInclude(ta => ta.Lesson)
                .Where(ta => ta.AppUserId == userId && ta.Theory.Lesson.GroupId == groupId && ta.IsRead == true)
                .ToListAsync();
        }

        public async Task<List<TheoryAppUser>> GetTheoryAppUsersByLessonIdAsync(int lessonId)
        {
            return await Context.TheoryAppUsers.AsNoTracking()
                .Include(ta => ta.Theory)
                .Include(ta => ta.AppUser)
                .Where(ta => ta.Theory.LessonId == lessonId && ta.IsRead == true)
                .ToListAsync();
        }

        public async Task<bool> InitializeTheoryAsync(List<AppUserGroup> appUserGroups, int theoryId)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                foreach (var appUserGroup in appUserGroups)
                {
                    TheoryAppUser theoryAppUser = new();

                    theoryAppUser.AppUserId = appUserGroup.AppUserId;
                    theoryAppUser.TheoryId = theoryId;
                    theoryAppUser.IsRead = false;

                    await Context.TheoryAppUsers.AddAsync(theoryAppUser);
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

        public async Task<bool> ReinitializeTheoriesAsync(List<AppUserGroup> appUserGroups, List<Theory> theories)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                foreach (var appUserGroup in appUserGroups)
                {
                    foreach (var theory in theories)
                    {
                        TheoryAppUser theoryAppUser = new();

                        theoryAppUser.AppUserId = appUserGroup.AppUserId;
                        theoryAppUser.TheoryId = theory.Id;
                        theoryAppUser.IsRead = false;

                        await Context.TheoryAppUsers.AddAsync(theoryAppUser);
                    }
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

    }
}

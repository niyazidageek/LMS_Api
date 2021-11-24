using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using MoreLinq.Extensions;

namespace DataAccess.Concrete
{
    public class EFAssignmentAppUserDal: EFRepositoryBase<AssignmentAppUser, AppDbContext>, IAssignmentAppUserDal
    {
        public EFAssignmentAppUserDal(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> InitializeAssignmentAsync(List<AppUserGroup> appUserGroups, int assignmentId)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                foreach (var appUserGroup in appUserGroups)
                {
                    AssignmentAppUser assignmentAppUser = new();

                    assignmentAppUser.AppUserId = appUserGroup.AppUserId;
                    assignmentAppUser.AssignmentId = assignmentId;
                    assignmentAppUser.IsSubmitted = false;
                    assignmentAppUser.SubmissionDate = null;

                    await Context.AssignmentAppUsers.AddAsync(assignmentAppUser);
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

        public async Task<bool> ReinitializeAssignmentsAsync(List<AppUserGroup> appUserGroups, List<Assignment> assignments)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                foreach (var appUserGroup in appUserGroups)
                {
                    foreach (var assignment in assignments)
                    {
                        AssignmentAppUser assignmentAppUser = new();

                        assignmentAppUser.AppUserId = appUserGroup.AppUserId;
                        assignmentAppUser.AssignmentId = assignment.Id;
                        assignmentAppUser.IsSubmitted = false;
                        assignmentAppUser.SubmissionDate = null;

                        await Context.AssignmentAppUsers.AddAsync(assignmentAppUser);
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

        public async Task<int> GetAssignmentAppUsersByLessonIdCountAsync(int lessonId)
        {
            return await Context.AssignmentAppUsers.AsNoTracking()
                .Include(aa=>aa.Assignment)
                .CountAsync(aa => aa.Assignment.LessonId == lessonId);
        }

        //Gets all submissions by lesson ID

        public async Task<List<AssignmentAppUser>> GetAssignmentAppUsersByLessonIdAsync(int lessonId)
        {
            return await Context.AssignmentAppUsers.AsNoTracking()
                .Include(aa=>aa.Assignment)
                .Include(aa=>aa.AppUser)
                .Include(aa=>aa.AssignmentAppUserMaterials)
                .Where(aa => aa.Assignment.LessonId == lessonId)
                .ToListAsync();
        }

        public async Task<AssignmentAppUser> GetAssignmentAppUserByAssignmentIdAndUserIdAsync(int assignmentId, string userId)
        {
            return await Context.AssignmentAppUsers.AsNoTracking()
                .Include(aa => aa.AssignmentAppUserMaterials)
                .FirstOrDefaultAsync(aa => aa.AppUserId == userId && aa.AssignmentId == assignmentId);
        }

        public async Task<AssignmentAppUser> GetAsync(int id)
        {
            return await Context.AssignmentAppUsers.AsNoTracking()
                .Include(aa=>aa.AppUser)
                .Include(aa => aa.AssignmentAppUserMaterials)
                .Include(aa=>aa.Assignment)
                .FirstOrDefaultAsync(aa => aa.Id == id);
        }

        public async Task<List<AssignmentAppUser>> GetAssignmentAppUsersByAppUserIdAndGroupIdAsync(string userId, int groupId)
        {
            return await Context.AssignmentAppUsers.AsNoTracking()
                .Include(aa => aa.Assignment)
                .ThenInclude(aa => aa.Lesson)
                .Where(aa => aa.AppUserId == userId && aa.Assignment.Lesson.GroupId == groupId && aa.IsSubmitted == true)
                .ToListAsync();
        }

        public async Task<List<int?>> GetAllPossibleSubmissionsCountByGroupIdAndYearAsync(int monthsCount, int groupId, int year)
        {
            List<int?> possibleSubmissionsCount = new();

            for (int i = 1; i <= monthsCount; i++)
            {
                int submissionsCountDb = await Context.AssignmentAppUsers
                    .AsNoTracking()
                    .Include(aa => aa.Assignment)
                    .ThenInclude(aa=>aa.Lesson)
                    .CountAsync(aa =>
                    aa.Assignment.Lesson.GroupId==groupId
                    && aa.Assignment.CreationDate.Year == year
                    && aa.Assignment.CreationDate.Month == i);

                if(submissionsCountDb == 0)
                {
                    possibleSubmissionsCount.Add(null);
                }
                else
                {
                    possibleSubmissionsCount.Add(submissionsCountDb);
                }

            }

            return possibleSubmissionsCount;
        }

        public async Task<List<int>> GetAllSubmissionsCountByGroupIdAndYearAsync(int monthsCount, int groupId, int year)
        {
            List<int> submissionsCount = new();

            for (int i = 1; i <= monthsCount; i++)
            {
                int submissionsCountDb = await Context.AssignmentAppUsers
                    .AsNoTracking()
                    .Include(aa => aa.Assignment)
                    .ThenInclude(aa => aa.Lesson)
                    .CountAsync(aa =>
                    aa.IsSubmitted==true
                    && aa.Assignment.Lesson.GroupId == groupId
                    && aa.Assignment.CreationDate.Year == year
                    && aa.Assignment.CreationDate.Month == i);

                submissionsCount.Add(submissionsCountDb);

            }

            return submissionsCount;
        }

        public async Task<List<int>> GetPossibleYearsAsync(int groupId)
        {
            var groupSubmissionsDb = await Context.AssignmentAppUsers
                .AsNoTracking()
                .Include(aa=>aa.Assignment)
                .ThenInclude(aa => aa.Lesson)
                .Where(aa => aa.Assignment.Lesson.GroupId == groupId)
                .ToListAsync();

            return groupSubmissionsDb.DistinctBy(gs => gs.Assignment.CreationDate.Year)
                .Select(gs => gs.Assignment.CreationDate.Year).ToList();
        }
    }
}

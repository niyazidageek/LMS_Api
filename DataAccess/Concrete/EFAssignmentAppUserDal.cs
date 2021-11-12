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
    public class EFAssignmentAppUserDal: EFRepositoryBase<AssignmentAppUser, AppDbContext>, IAssignmentAppUserDal
    {
        public EFAssignmentAppUserDal(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> InitializeAssignmentAsync(Lesson lesson, int assignmentId)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                foreach (var appUserGroup in lesson.Group.AppUserGroups)
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

        //Gets all submissions by lesson ID

        public async Task<List<AssignmentAppUser>> GetAssignmentAppUsersByLessonIdAsync(int lessonId)
        {
            return await Context.AssignmentAppUsers.AsNoTracking()
                .Include(aa=>aa.Assignment)
                .Include(aa=>aa.AppUser)
                .Include(aa=>aa.AssignmentAppUserMaterials)
                .Where(aa => aa.Assignment.LessonId == lessonId &&  aa.IsSubmitted == true)
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
    }
}

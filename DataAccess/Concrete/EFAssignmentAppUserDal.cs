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

        public async Task<List<AssignmentAppUser>> GetAssignmentAppUsersByLessonIdAndUserIdAsync(int lessonId, string appUserId)
        {
            return await Context.AssignmentAppUsers.AsNoTracking()
                .Include(aa=>aa.Assignment)
                .ThenInclude(aa=>aa.LessonId)
                .Include(aa=>aa.AssignmentAppUserMaterials)
                .Where(aa => aa.AppUserId == appUserId && aa.Assignment.LessonId == lessonId &&  aa.IsSubmitted == true)
                .ToListAsync();
        }
    }
}

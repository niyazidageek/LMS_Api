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
    public class EFAssignmentDal: EFRepositoryBase<Assignment, AppDbContext>, IAssignmentDal
    {
        public EFAssignmentDal(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Assignment>> GetAllByGroupIdAsync(int groupId)
        {
            return await Context.Assignments
                .AsNoTracking()
                .Include(a => a.Lesson)
                .Where(a => a.Lesson.GroupId == groupId)
                .ToListAsync();
        }

        public async Task<List<Assignment>> GetAllByLessonIdAsync(int lessonId)
        {
            return await Context.Assignments
                .AsNoTracking()
                .Where(a => a.LessonId == lessonId)
                .ToListAsync();
        }

        public async Task<List<Assignment>> GetAllByGroupIdAsync(int groupId, int page=0, int size = 3)
        {
            return await Context.Assignments.AsNoTracking()
                .Include(a => a.Lesson)
                .Where(a => a.Lesson.GroupId == groupId)
                .OrderByDescending(a=>a.Deadline)
                .Skip(page * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<Assignment> GetByIdAsync(int assignmentId)
        {
            return await Context.Assignments
                .AsNoTracking()
                .Include(a => a.AssignmentMaterials)
                .Include(a => a.Lesson)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);
        }

        public async Task<int> GetAssignmentsByGroupIdCountAsync(int groupId)
        {
            return await Context.Assignments.AsNoTracking()
                 .Include(a=>a.Lesson)
                 .CountAsync(a => a.Lesson.GroupId == groupId);
        }

        public async Task<Assignment> GetByIdAndUserIdAsync(int assignmentId, string userId)
        {
            return await Context.Assignments
                .AsNoTracking()
                .Include(a => a.AssignmentMaterials)
                .Include(a=>a.AssignmentAppUsers.Where(aa=>aa.AppUserId == userId))
                .FirstOrDefaultAsync(a => a.Id == assignmentId);
        }

        public async Task<List<Assignment>> GetAssignmentsByLessonIdAndUserIdAsync(int lessonId, string appUserId)
        {
            var assignmentAppUsers = await Context.AssignmentAppUsers.AsNoTracking()
                .Include(aa => aa.Assignment)
                .Where(aa => aa.AppUserId == appUserId && aa.Assignment.LessonId == lessonId)
                .ToListAsync();

            List<Assignment> assignmentsDb = new();

            foreach (var assignmentAppUser in assignmentAppUsers)
            {
                var assignmentDb = await Context.Assignments.AsNoTracking()
                    .Include(a=>a.AssignmentAppUsers.Where(aa=>aa.AppUserId == appUserId))
                    .FirstOrDefaultAsync(a => a.Id == assignmentAppUser.AssignmentId);
                assignmentsDb.Add(assignmentDb);
            }

            return assignmentsDb;
        }
    }
}

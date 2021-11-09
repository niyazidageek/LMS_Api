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

        public async Task<List<Assignment>> GetAllByLessonIdAsync(int lessonId)
        {
            return await Context.Assignments
                .Where(a => a.LessonId == lessonId)
                .Include(a => a.AssignmentMaterials)
                .ToListAsync();
        }

        public async Task<List<Assignment>> GetAssignmentsByLessonIdAndUserIdAsync(int lessonId, string appUserId)
        {
            var assignmentAppUsers = await Context.AssignmentAppUsers.AsNoTracking()
                .Include(aa => aa.Assignment)
                .ThenInclude(aa => aa.LessonId)
                .Where(aa => aa.AppUserId == appUserId && aa.Assignment.LessonId == lessonId && aa.IsSubmitted == false)
                .ToListAsync();

            List<Assignment> assignmentsDb = new();

            foreach (var assignmentAppUser in assignmentAppUsers)
            {
                var assignmentDb = await Context.Assignments.AsNoTracking()
                    .Include(a => a.AssignmentMaterials)
                    .FirstOrDefaultAsync(a => a.Id == assignmentAppUser.AssignmentId);
                assignmentsDb.Add(assignmentDb);
            }

            return assignmentsDb;
        }
    }
}

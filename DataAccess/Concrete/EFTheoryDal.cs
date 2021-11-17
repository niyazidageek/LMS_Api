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
    public class EFTheoryDal:EFRepositoryBase<Theory, AppDbContext>, ITheoryDal
    {
        public EFTheoryDal(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Theory>> GetAllByGroupIdAsync(int groupId)
        {
            return await Context.Theories
                .AsNoTracking()
                .Include(t => t.Lesson)
                .Where(t => t.Lesson.GroupId == groupId)
                .ToListAsync();
        }

        public async Task<Theory> GetByIdAndUserId(int theoryId, string userId)
        {
            return await Context.Theories.AsNoTracking()
                .Include(t => t.TheoryAppUsers.Where(ta => ta.AppUserId == userId))
                .FirstOrDefaultAsync(t => t.Id == theoryId);
        }

        public async Task<List<Theory>> GetAllByLessonIdAsync(int lessonId)
        {
            return await Context.Theories.AsNoTracking()
                .Where(t => t.LessonId == lessonId)
                .ToListAsync();
        }

        public async Task<List<Theory>> GetTheoriesByLessonIdAndUserIdAsync(int lessonId, string appUserId)
        {
            var theoryAppUsers = await Context.TheoryAppUsers.AsNoTracking()
                .Include(ta => ta.Theory)
                .Where(ta => ta.AppUserId == appUserId && ta.Theory.LessonId == lessonId)
                .ToListAsync();

            List<Theory> theoriesDb = new();

            foreach (var theoryAppUser in theoryAppUsers)
            {
                var theoryDb = await Context.Theories.AsNoTracking()
                    .Include(t => t.TheoryAppUsers.Where(ta => ta.AppUserId == appUserId))
                    .FirstOrDefaultAsync(t => t.Id == theoryAppUser.TheoryId);

                theoriesDb.Add(theoryDb);
            }

            return theoriesDb;
        }
    }
}

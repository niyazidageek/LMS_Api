using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.DTOs;
using Entities.Models;
using LMS_Api.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class EFLessonDal: EFRepositoryBase<Lesson, AppDbContext>, ILessonDal
    {
        public EFLessonDal(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Lesson>> GetAllAsync()
        {
            return await Context.Lessons.AsNoTracking()
                .Include(l => l.Group)
                .ToListAsync();
        }

        public async Task<Lesson> GetAsync(int id)
        {
            return await Context.Lessons.AsNoTracking()
                .Include(l=>l.Assignments)
                .Include(l=>l.Theories)
                .Include(l => l.Group)
                .ThenInclude(l=>l.AppUserGroups)
                .FirstOrDefaultAsync(l => l.Id == id);
        }



        public async Task<List<Lesson>> GetAllByGroupIdAsync(int groupId)
        {
            return await Context.Lessons.AsNoTracking()
                .Where(l => l.GroupId == groupId)
                .Include(l=>l.Theories)
                .Include(l=>l.Assignments)
                .ThenInclude(l => l.AssignmentMaterials)
                .ToListAsync();
        }

        public async Task<List<Lesson>> GetLessonsByGroupIdAndUserIdAsync(int groupId, string userId, int page=0, int size=3)
        {
            return await Context.Lessons.AsNoTracking()
                .Include(l => l.Assignments)
                .ThenInclude(l => l.AssignmentAppUsers.Where(aa => aa.AppUserId == userId && aa.IsSubmitted == true))
                .Include(l => l.Theories)
                .ThenInclude(l => l.TheoryAppUsers.Where(ta => ta.AppUserId == userId && ta.IsRead == true))
                .Where(l => l.GroupId == groupId && l.StartDate<=DateTime.UtcNow.AddDays(2))
                .OrderByDescending(l=>l.StartDate)
                .Skip(page*size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> GetLessonsByGroupIdCountAsync(int groupId)
        {
            return await Context.Lessons.AsNoTracking()
                 .CountAsync(l => l.GroupId == groupId);
        }

        public async Task<List<Lesson>> GetAllByGroupIdAsync(int groupId, int skip=0, int take=2)
        {
            return await Context.Lessons.AsNoTracking()
                .Where(l => l.GroupId == groupId)
                .OrderByDescending(l=>l.Id)
                .Skip(skip)
                .Take(take)
                .Include(l => l.Assignments)
                .ThenInclude(l=>l.AssignmentMaterials)
                .Include(l => l.Theories)
                .ToListAsync();
        }
    }
}

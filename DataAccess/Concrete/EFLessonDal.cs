﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                .Include(l=>l.LessonJoinLink)
                .Include(l => l.Group)
                .ThenInclude(l=>l.AppUserGroups)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List<Lesson>> GetAllByMatchAndGroupIdAsync(int groupId,string match)
        {
            return await Context.Lessons.AsNoTracking()
                .Where(l => l.Name.Contains(match) && l.GroupId == groupId)
                .ToListAsync();
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

        public async Task<List<Lesson>> GetLessonsByGroupIdAndUserIdAsync(string userId, int page=0, int size=3,
            Expression<Func<Lesson, bool>> filter = null)
        {
            return await Context.Lessons.AsNoTracking()
                .Include(l => l.LessonJoinLink)
                .Include(l => l.Assignments)
                .ThenInclude(l => l.AssignmentAppUsers.Where(aa => aa.AppUserId == userId && aa.IsSubmitted == true))
                .Include(l => l.Theories)
                .ThenInclude(l => l.TheoryAppUsers.Where(ta => ta.AppUserId == userId && ta.IsRead == true))
                .Where(filter)
                .OrderByDescending(l=>l.StartDate)
                .Skip(page*size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<List<Lesson>> GetLessonsByGroupIdWithSubmissionsAsync(int groupId, int page = 0, int size = 3)
        {
            return await Context.Lessons.AsNoTracking()
                .Include(l => l.Assignments)
                .ThenInclude(l => l.AssignmentAppUsers.Where(aa=>aa.IsSubmitted == true))
                .Where(l=>l.GroupId == groupId && (l.Assignments.Count!=0 && l.Assignments != null))
                .OrderByDescending(l => l.StartDate)
                .Skip(page * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> GetLessonsByGroupIdCountAsync(int groupId)
        {
            return await Context.Lessons.AsNoTracking()
                 .CountAsync(l => l.GroupId == groupId);
        }

        public async Task<int> GetLessonsByGroupIdWithSubmissionsCountAsync(int groupId)
        {
            return await Context.Lessons.AsNoTracking()
                .Include(l=>l.Assignments)
                .CountAsync(l => l.GroupId == groupId && (l.Assignments.Count != 0 && l.Assignments != null));
        }

        public async Task<List<Lesson>> GetAllByGroupIdAsync(int page=0, int size=3,
             Expression<Func<Lesson, bool>> filter = null)
        {
            return await Context.Lessons.AsNoTracking()
                .Include(l => l.LessonJoinLink)
                .Include(l => l.Assignments)
                .Include(l => l.Theories)
                .Where(filter)
                .OrderByDescending(l => l.StartDate)
                .Skip(page*size)
                .Take(size)
                .ToListAsync();
        }
    }
}

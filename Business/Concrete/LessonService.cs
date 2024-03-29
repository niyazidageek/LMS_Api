﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;

namespace Business.Concrete
{
    public class LessonService : ILessonService
    {
        private readonly ILessonDal _conext;

        public LessonService(ILessonDal conext)
        {
            _conext = conext;
        }

        public async Task<bool> AddLessonAsync(Lesson lesson)
        {
            await _conext.AddAsync(lesson);

            return true;
        }

        public async Task<bool> DeleteLessonAsync(int id)
        {
            await _conext.DeleteAsync(new Lesson { Id = id });

            return true;
        }

        public async Task<bool> EditLessonAsync(Lesson lesson)
        {
            await _conext.UpdateAsync(lesson);

            return true;
        }

        public async Task<Lesson> GetLessonByIdAsync(int id)
        {
            return await _conext.GetAsync(id);
        }

        public async Task<List<Lesson>> GetLessonsAsync()
        {
            return await _conext.GetAllAsync();
        }

        public async Task<List<Lesson>> GetLessonsByGroupIdAndUserIdAsync(int groupId, string userId, int page = 0,
            int size = 3, int futureDaysCount=2)
        {
            return await _conext.GetLessonsByGroupIdAndUserIdAsync(userId, page, size,
                l=>l.GroupId == groupId && l.StartDate <= DateTime.UtcNow.AddDays(futureDaysCount));
        }

        public async Task<List<Lesson>> GetLessonsByGroupIdAndUserIdAsync(int groupId, string userId, int page = 0,
            int size = 3)
        {
            return await _conext.GetLessonsByGroupIdAndUserIdAsync(userId, page, size,
                l => l.GroupId == groupId);
        }

        public async Task<List<Lesson>> GetLessonsByGroupIdAsync(int groupId)
        {
            return await _conext.GetAllByGroupIdAsync(groupId);
        }

        public async Task<List<Lesson>> GetLessonsByGroupIdAsync(int groupId, int page = 0, int size = 3)
        {
            return await _conext.GetAllByGroupIdAsync(page, size,
                l=>l.GroupId == groupId);
        }

        public async Task<List<Lesson>> GetLessonsByGroupIdAsync(int groupId, int page = 0, int size = 3, int futureDaysCount = 2)
        {
            return await _conext.GetAllByGroupIdAsync(page, size,
                l=>l.GroupId == groupId && l.StartDate <= DateTime.UtcNow.AddDays(futureDaysCount));
        }

        public async Task<int> GetLessonsByGroupIdCountAsync(int groupId)
        {
            return await _conext.GetLessonsByGroupIdCountAsync(groupId);
        }

        public async Task<List<Lesson>> GetLessonsByGroupIdWithSubmissionsAsync(int groupId, int page = 0, int size = 3)
        {
            return await _conext.GetLessonsByGroupIdWithSubmissionsAsync(groupId, page, size);
        }

        public async Task<int> GetLessonsByGroupIdWithSubmissionsCountAsync(int groupId)
        {
            return await _conext.GetLessonsByGroupIdWithSubmissionsCountAsync(groupId);
        }

        public async Task<List<Lesson>> GetLessonsByMatchAndGroupIdAsync(int groupId, string match)
        {
            return await _conext.GetAllByMatchAndGroupIdAsync(groupId, match);
        }
    }
}

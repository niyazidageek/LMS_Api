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

        public async Task<List<Lesson>> GetLessonsByGroupIdAndUserIdAsync(int groupId, string userId, int page = 0, int size = 3)
        {
            return await _conext.GetLessonsByGroupIdAndUserIdAsync(groupId, userId, page, size);
        }

        public async Task<List<Lesson>> GetLessonsByGroupIdAsync(int groupId)
        {
            return await _conext.GetAllByGroupIdAsync(groupId);
        }

        public async Task<List<Lesson>> GetLessonsByGroupIdAsync(int groupId, int skip = 0, int take = 2)
        {
            return await _conext.GetAllByGroupIdAsync(groupId, skip, take);
        }

        public async Task<int> GetLessonsByGroupIdCountAsync(int groupId)
        {
            return await _conext.GetLessonsByGroupIdCountAsync(groupId);
        }
    }
}

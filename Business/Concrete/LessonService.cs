﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

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

        public async Task<bool> AddLessonAsync(Lesson lesson, List<string> fileNames)
        {
            await _conext.AddWithFilesAsync(lesson, fileNames);

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
            return await _conext.GetAsync(l => l.Id == id);
        }

        public async Task<List<Lesson>> GetLessonsAsync()
        {
            return await _conext.GetAllAsync();
        }
    }
}

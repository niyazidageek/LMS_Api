using System;
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

        public async Task<bool> AddLessonAsync(Lesson lesson, List<IFormFile> files)
        {
            await _conext.AddWithFilesAsync(lesson, files);

            return true;
        }

        public async Task<bool> DeleteLessonAsync(int id)
        {
            await _conext.DeleteWithFilesAsync(new Lesson { Id = id });

            return true;
        }

        public async Task<bool> EditLessonAsync(Lesson lesson, List<IFormFile> files, List<MaterialDTO> existingMaterialsDto)
        {
            await _conext.EditWithFilesAsync(lesson, files, existingMaterialsDto);

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

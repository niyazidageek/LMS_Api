using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;

namespace Business.Abstract
{
    public interface ILessonService
    {
        Task<List<Lesson>> GetLessonsAsync();

        Task<Lesson> GetLessonByIdAsync(int id);

        Task<bool> AddLessonAsync(Lesson lesson);

        Task<bool> AddLessonAsync(Lesson lesson, List<IFormFile> files);

        Task<bool> EditLessonAsync(Lesson lesson, List<IFormFile> files, List<MaterialDTO> existingMaterialsDto);

        Task<bool> DeleteLessonAsync(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface ILessonService
    {
        Task<List<Lesson>> GetLessonsAsync();

        Task<Lesson> GetLessonByIdAsync(int id);

        Task<bool> AddLessonAsync(Lesson lesson);

        Task<bool> EditLessonAsync(Lesson lesson);

        Task<bool> DeleteLessonAsync(int id);
    }
}

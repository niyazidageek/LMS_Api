﻿using System;
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

        //Task<bool> AddLessonWithFilesAsync(Lesson lesson);

        Task<bool> EditLessonAsync(Lesson lesson);

        //Task<bool> EditLessonWithFilesAsync(Lesson lesson);

        Task<bool> DeleteLessonAsync(int id);

        Task<List<Lesson>> GetLessonsByGroupIdAsync(int groupId);

        Task<List<Lesson>> GetLessonsByGroupIdAsync(int groupId, int skip = 0, int take = 2);
    }
}

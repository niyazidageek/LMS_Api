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

        Task<List<Lesson>> GetLessonsByMatchAndGroupIdAsync(int groupId,string match);

        Task<bool> AddLessonAsync(Lesson lesson);

        Task<bool> EditLessonAsync(Lesson lesson);

        Task<bool> DeleteLessonAsync(int id);

        Task<int> GetLessonsByGroupIdCountAsync(int groupId);

        Task<List<Lesson>> GetLessonsByGroupIdAsync(int groupId);

        Task<List<Lesson>> GetLessonsByGroupIdAsync(int groupId, int page = 0, int size = 3);

        Task<List<Lesson>> GetLessonsByGroupIdAsync(int groupId, int page = 0, int size = 3, int futureDaysCount = 2);

        Task<List<Lesson>> GetLessonsByGroupIdAndUserIdAsync(int groupId, string userId, int page = 0,
            int size = 3, int futureDaysCount=2);

        Task<List<Lesson>> GetLessonsByGroupIdAndUserIdAsync(int groupId, string userId, int page = 0, int size = 3);

        Task<List<Lesson>> GetLessonsByGroupIdWithSubmissionsAsync(int groupId, int page = 0, int size = 3);
    }
}

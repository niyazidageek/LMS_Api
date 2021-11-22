using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Repository;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;

namespace DataAccess.Abstract
{
    public interface ILessonDal:IRepository<Lesson>
    {
        Task<List<Lesson>> GetAllAsync();

        Task<List<Lesson>> GetAllByMatchAndGroupIdAsync(int groupId, string match);

        Task<List<Lesson>> GetAllByGroupIdAsync(int groupId);

        Task<Lesson> GetAsync(int id);

        Task<List<Lesson>> GetLessonsByGroupIdAndUserIdAsync(string userId, int page = 0, int size = 3,
            Expression<Func<Lesson, bool>> filter = null);

        Task<List<Lesson>> GetAllByGroupIdAsync(int page = 0, int size = 3,
             Expression<Func<Lesson, bool>> filter = null);

        Task<int> GetLessonsByGroupIdCountAsync(int groupId);

        Task<List<Lesson>> GetLessonsByGroupIdWithSubmissionsAsync(int groupId, int page = 0, int size = 3);
    }
}

using System;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface ILessonJoinLinkDal : IRepository<LessonJoinLink>
    {
        Task<LessonJoinLink> GetByLessonIdAsync(int lessonId);
    }
}

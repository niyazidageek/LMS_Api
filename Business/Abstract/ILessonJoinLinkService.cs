using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface ILessonJoinLinkService
    {
        Task<List<LessonJoinLink>> GetLessonJoinLinksAsync();

        Task<LessonJoinLink> GetLessonJoinLinkByIdAsync(int id);

        Task<LessonJoinLink> GetLessonJoinLinkByLessonIdAsync(int lessonId);

        Task<bool> AddLessonJoinLinkAsync(LessonJoinLink lessonJoinLink);

        Task<bool> EditLessonJoinLinkAsync(LessonJoinLink lessonJoinLink);

        Task<bool> DeleteLessonJoinLinkAsync(int id);

        Task<bool> HasLessonStartedByLessonIdAsync(int lessonId);
    }
}

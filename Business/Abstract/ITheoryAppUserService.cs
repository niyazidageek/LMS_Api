using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface ITheoryAppUserService
    {
        Task<List<TheoryAppUser>> GetTheoryAppUsersByLessonIdAsync(int lessonId);

        Task<TheoryAppUser> GetTheoryAppUserByIdAsync(int id);

        Task<bool> InitializeTheoryAsync(Lesson lesson, int theoryId);

        Task<bool> EditTheoryAppUserAsync(TheoryAppUser theoryAppUser);

        Task<TheoryAppUser> GetTheoryAppUserByTheoryIdAndUserIdAsync(int theoryId, string userId);
    }
}

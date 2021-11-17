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

        Task<bool> InitializeTheoryAsync(List<AppUserGroup> appUserGroups, int theoryId);

        Task<bool> EditTheoryAppUserAsync(TheoryAppUser theoryAppUser);

        Task<TheoryAppUser> GetTheoryAppUserByTheoryIdAndUserIdAsync(int theoryId, string userId);

        Task<List<TheoryAppUser>> GetTheoryAppUsersByAppUserIdAndGroupId(string userId, int groupId);

        Task<bool> ReinitializeTheoriesAsync(List<AppUserGroup> appUserGroups, List<Theory> theories);
    }
}

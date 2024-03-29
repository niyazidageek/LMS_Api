﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface ITheoryAppUserDal: IRepository<TheoryAppUser>
    {
        Task<bool> InitializeTheoryAsync(List<AppUserGroup> appUserGroups, int theoryId);

        Task<List<TheoryAppUser>> GetTheoryAppUsersByLessonIdAsync(int lessonId);

        Task<List<TheoryAppUser>> GetTheoryAppUsersByAppUserIdAndGroupIdAsync(string userId, int groupId);

        Task<TheoryAppUser> GetTheoryAppUserByTheoryIdAndUserIdAsync(int theoryId, string userId);

        Task<bool> ReinitializeTheoriesAsync(List<AppUserGroup> appUserGroups, List<Theory> theories);
    }
}

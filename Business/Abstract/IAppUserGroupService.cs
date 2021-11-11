using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IAppUserGroupService
    {
        Task<bool> CreateAppUserGroupsAsync(List<AppUserGroup> appUserGroups);

        Task<bool> DeleteAppUserGroupsAsync(List<AppUserGroup> appUserGroups);

        Task<AppUserGroup> GetAppUserGroupByUserIdAndGroupIdAsync(string userId, int groupId);
    }
}

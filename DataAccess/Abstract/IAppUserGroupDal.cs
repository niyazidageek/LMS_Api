using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IAppUserGroupDal: IRepository<AppUserGroup>
    {
        Task<bool> CreateAppUserGroupsAsync(List<AppUserGroup> appUserGroups);

        Task<bool> DeleteAppUserGroupsAsync(List<AppUserGroup> appUserGroups);

        Task<AppUserGroup> GetByAppUserIdAndGroupIdAsync(string userId, int groupId);

        Task<List<AppUserGroup>> GetAllByGroupIdAsync(int groupId);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class AppUserGroupService:IAppUserGroupService
    {
        private IAppUserGroupDal _context;

        public AppUserGroupService(IAppUserGroupDal context)
        {
            _context = context;
        }

        public async Task<bool> CreateAppUserGroupsAsync(List<AppUserGroup> appUserGroups)
        {
            await _context.CreateAppUserGroupsAsync(appUserGroups);

            return true;
        }

        public async Task<bool> DeleteAppUserGroupsAsync(List<AppUserGroup> appUserGroups)
        {
            await _context.DeleteAppUserGroupsAsync(appUserGroups);

            return true;
        }

        public async Task<AppUserGroup> GetAppUserGroupByUserIdAndGroupIdAsync(string userId, int groupId)
        {
            return await _context.GetByAppUserIdAndGroupIdAsync(userId, groupId);
        }
    }
}

using System;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IAppUserGroupPointService
    {
        Task<AppUserGroupPoint> GetAppUserGroupPointByAppUserGroupIdAsync(int appUserGroup);

        Task<bool> EditAppUserGroupPointAsync(AppUserGroupPoint appUserGroupPoint);

    }
}

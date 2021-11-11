using System;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IAppUserGroupPointDal: IRepository<AppUserGroupPoint>
    {
        Task<AppUserGroupPoint> GetAppUserGroupPointByAppUserGroupIdAsync(int appUserGroup);
    }
}

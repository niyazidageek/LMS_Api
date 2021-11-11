using System;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class AppUserGroupPointService:IAppUserGroupPointService
    {
        private readonly IAppUserGroupPointDal _context;

        public AppUserGroupPointService(IAppUserGroupPointDal context)
        {
            _context = context;
        }

        public async Task<bool> EditAppUserGroupPointAsync(AppUserGroupPoint appUserGroupPoint)
        {
            await _context.UpdateAsync(appUserGroupPoint);

            return true;
        }

        public async Task<AppUserGroupPoint> GetAppUserGroupPointByAppUserGroupIdAsync(int appUserGroup)
        {
            return await _context.GetAppUserGroupPointByAppUserGroupIdAsync(appUserGroup);
        }
    }
}

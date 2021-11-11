using System;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class EFAppUserGroupPointDal : EFRepositoryBase<AppUserGroupPoint, AppDbContext>, IAppUserGroupPointDal
    {
        public EFAppUserGroupPointDal(AppDbContext context) : base(context)
        {
        }

        public async Task<AppUserGroupPoint> GetAppUserGroupPointByAppUserGroupIdAsync(int appUserGroup)
        {
            return await Context.AppUserGroupPoints.AsNoTracking()
                .FirstOrDefaultAsync(agp => agp.AppUserGroupId == appUserGroup);
        }
    }
}

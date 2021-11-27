using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class EFAppUserNotificationDal : EFRepositoryBase<AppUserNotification, AppDbContext>, IAppUserNotificationDal
    {
        public EFAppUserNotificationDal(AppDbContext context) : base(context)
        {
            
        }

        public async Task<List<AppUserNotification>> GetAllUnreadByUserIdAsync(string userId)
        {
            return await Context.AppUserNotifications
                .AsNoTracking()
                .Where(an => an.IsRead == false && an.AppUserId == userId)
                .Include(an=>an.Notification)
                .OrderByDescending(an => an.Notification.CreationDate)
                .ToListAsync();
        }
    }
}

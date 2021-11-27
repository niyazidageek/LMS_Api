using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class AppUserNotificationService:IAppUserNotificationService
    {
        private readonly IAppUserNotificationDal _context;

        public AppUserNotificationService(IAppUserNotificationDal context)
        {
            _context = context;
        }

        public async Task<bool> AddAppUserNotificationAsync(AppUserNotification appUserNotification)
        {
            await _context.AddAsync(appUserNotification);

            return true;
        }

        public async Task<bool> EditAppUserNotificationAsync(AppUserNotification appUserNotification)
        {
            await _context.UpdateAsync(appUserNotification);

            return true;
        }

        public async Task<AppUserNotification> GetAppUserNotificationById(int id)
        {
            return await _context.GetAsync(an => an.Id == id);
        }

        public async Task<List<AppUserNotification>> GetUnreadAppUserNotificationsByUserIdAsync(string userId)
        {
            return await _context.GetAllUnreadByUserIdAsync(userId);
        }
    }
}

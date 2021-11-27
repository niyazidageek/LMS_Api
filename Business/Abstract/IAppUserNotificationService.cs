using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IAppUserNotificationService
    {
        Task<List<AppUserNotification>> GetUnreadAppUserNotificationsByUserIdAsync(string userId);

        Task<bool> AddAppUserNotificationAsync(AppUserNotification appUserNotification);

        Task<bool> EditAppUserNotificationAsync(AppUserNotification appUserNotification);

        Task<AppUserNotification> GetAppUserNotificationById(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IAppUserNotificationDal:IRepository<AppUserNotification>
    {
        Task<List<AppUserNotification>> GetAllUnreadByUserIdAsync(string userId);
    }
}

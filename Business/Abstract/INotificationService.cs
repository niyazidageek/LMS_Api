using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface INotificationService
    {
        Task<bool> AddNotificationAsync(Notification notification);
    }
}

using System;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationDal _context;

        public NotificationService(INotificationDal context)
        {
            _context = context;
        }

        public async Task<bool> AddNotificationAsync(Notification notification)
        {
            await _context.AddAsync(notification);

            return true;
        }
    }
}

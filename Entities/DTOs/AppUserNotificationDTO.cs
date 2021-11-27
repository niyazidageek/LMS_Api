using System;
namespace Entities.DTOs
{
    public class AppUserNotificationDTO
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }
        public AppUserDTO AppUser { get; set; }

        public int NotificationId { get; set; }
        public NotificationDTO Notification { get; set; }

        public bool IsRead { get; set; }
    }
}

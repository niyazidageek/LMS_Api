using System;
using System.Collections.Generic;
using Core.Entities;

namespace Entities.Models
{
    public class Notification:IEntity
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreationDate { get; set; }

        public List<AppUserNotification> AppUserNotifications { get; set; }
    }
}

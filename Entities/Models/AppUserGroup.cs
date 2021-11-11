using System;
using Core.Entities;

namespace Entities.Models
{
    public class AppUserGroup:IEntity
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public AppUserGroupPoint AppUserGroupPoint { get; set; }
    }
}

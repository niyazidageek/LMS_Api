using System;
namespace Entities.Models
{
    public class AppUserGroup
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}

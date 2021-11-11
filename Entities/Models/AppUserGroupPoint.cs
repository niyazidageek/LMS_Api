using System;
using Core.Entities;

namespace Entities.Models
{
    public class AppUserGroupPoint:IEntity
    {
        public int Id { get; set; }

        public int AppUserGroupId { get; set; }
        public decimal Point { get; set; } = 0;

        public AppUserGroup AppUserGroup { get; set; }
    }
}

using System;
using Core.Entities;

namespace Entities.Models
{
    public class TheoryAppUser:IEntity
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int TheoryId { get; set; }
        public Theory Theory { get; set; }

        public bool IsRead { get; set; }
    }
}

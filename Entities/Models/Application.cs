using System;
using Core.Entities;

namespace Entities.Models
{
    public class Application:IEntity
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public string Message { get; set; }

        public DateTime CreationDate { get; set; }
    }
}

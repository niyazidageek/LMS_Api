using System;
namespace Entities.DTOs
{
    public class ApplicationDTO
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }
        public AppUserDTO AppUser { get; set; }

        public string Message { get; set; }

        public DateTime? CreationDate { get; set; }
    }
}

using System;
namespace Entities.DTOs
{
    public class TheoryAppUserDTO
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }
        public AppUserDTO AppUser { get; set; }

        public int TheoryId { get; set; }
        public TheoryDTO Theory { get; set; }

        public bool IsRead { get; set; }
    }
}

using System;
namespace Entities.DTOs
{
    public class AppUserOptionDTO
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }
        public AppUserDTO AppUser { get; set; }

        public int OptionId { get; set; }
        public OptionDTO Option { get; set; }

        public int QuestionId { get; set; }
        public QuestionDTO Question { get; set; }
    }
}

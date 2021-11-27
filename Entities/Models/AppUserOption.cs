using System;
using Core.Entities;

namespace Entities.Models
{
    public class AppUserOption:IEntity
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int? OptionId { get; set; }
        public Option Option { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}

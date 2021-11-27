using System;
namespace Entities.DTOs
{
    public class QuizMaxPointDTO
    {
        public int Id { get; set; }

        public int QuizId { get; set; }

        public decimal MaxPoint { get; set; } = 0;
    }
}

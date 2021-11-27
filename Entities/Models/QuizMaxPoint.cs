using System;
using Core.Entities;

namespace Entities.Models
{
    public class QuizMaxPoint:IEntity
    {
        public int Id { get; set; }

        public int QuizId { get; set; }
        public decimal MaxPoint { get; set; } = 0;

        public Quiz Quiz { get; set; }
    }
}

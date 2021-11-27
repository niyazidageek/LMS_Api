using System;
using Core.Entities;

namespace Entities.Models
{
    public class AppUserQuiz : IEntity
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public bool IsSubmitted { get; set; }

        public DateTime? SubmissionDate { get; set; }

        public bool isLate { get; set; }

        public decimal Result { get; set; }

        public int CorrectAnswers { get; set; }
    }
}

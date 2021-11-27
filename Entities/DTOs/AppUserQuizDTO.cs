using System;
namespace Entities.DTOs
{
    public class AppUserQuizDTO
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }
        public AppUserDTO AppUser { get; set; }

        public int QuizId { get; set; }
        public QuizDTO Quiz { get; set; }

        public bool IsSubmitted { get; set; }

        public DateTime? SubmissionDate { get; set; }

        public bool isLate { get; set; }

        public decimal Result { get; set; }

        public int CorrectAnswers { get; set; }
    }
}

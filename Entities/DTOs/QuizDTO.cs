using System;
using System.Collections.Generic;

namespace Entities.DTOs
{
    public class QuizDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int GroupId { get; set; }
        public GroupDTO Group { get; set; }

        public List<QuestionDTO> Questions { get; set; }

        public bool IsAvailable { get; set; }

        public List<AppUserQuizDTO> AppUserQuizzes { get; set; }

        public DateTime Deadline { get; set; }

        public QuizMaxPointDTO QuizMaxPoint { get; set; }

        public List<AppUserOptionDTO> AppUserOptions { get; set; }

        public bool NotifyAll { get; set; }

        public int DurationSeconds { get; set; }

        public int QuestionCount { get; set; }
    }
}

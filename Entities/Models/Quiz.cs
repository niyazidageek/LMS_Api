using System;
using System.Collections.Generic;
using Core.Entities;

namespace Entities.Models
{
    public class Quiz : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Group Group { get; set; }
        public int GroupId { get; set; }

        public DateTime Deadline { get; set; }

        public bool IsAvailable { get; set; }

        public List<AppUserQuiz> AppUserQuizzes { get; set; }

        public List<Question> Questions { get; set; }

        public int DurationSeconds { get; set; }

        public QuizMaxPoint QuizMaxPoint { get; set; }
    }
}

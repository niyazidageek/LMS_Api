using System;
using System.Collections.Generic;
using Core.Entities;

namespace Entities.Models
{
    public class Quiz : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SubjectId { get; set; }

        public Subject Subject { get; set; }

        public List<AppUserQuiz> AppUserQuizzes { get; set; }
    }
}

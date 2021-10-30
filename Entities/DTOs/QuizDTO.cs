using System;
namespace Entities.DTOs
{
    public class QuizDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SubjectId { get; set; }

        public SubjectDTO Subject { get; set; }
    }
}

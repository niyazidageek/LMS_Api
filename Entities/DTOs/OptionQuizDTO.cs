using System;
namespace Entities.DTOs
{
    public class OptionQuizDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int QuestionId { get; set; }

        public QuestionDTO Question { get; set; }

        public string FileName { get; set; }
    }
}

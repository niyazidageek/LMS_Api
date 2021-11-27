using System;
using System.Collections.Generic;

namespace Entities.DTOs
{
    public class QuestionQuizDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Point { get; set; }

        public int QuizId { get; set; }

        public QuizDTO Quiz { get; set; }

        public List<OptionQuizDTO> Options { get; set; }

        public string FileName { get; set; }
    }
}

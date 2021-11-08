using System;
using System.Collections.Generic;

namespace Entities.DTOs
{
    public class QuestionDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Point { get; set; }

        public int QuizId { get; set; }

        public QuizDTO Quiz { get; set; }

        public List<OptionDTO> Options { get; set; }

        public int OptionsCount { get; set; }

        public string FileName { get; set; }
    }
}

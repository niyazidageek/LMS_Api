using System;
using System.Collections.Generic;

namespace Entities.DTOs
{
    public class QuizAnswerDTO
    {
        public int QuestionId { get; set; }

        public int? OptionId { get; set; }
    }
}

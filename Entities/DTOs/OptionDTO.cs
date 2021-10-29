using System;
namespace Entities.DTOs
{
    public class OptionDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsCorrect { get; set; }

        public QuestionDTO Question { get; set; }

        public string FileName { get; set; }
    }
}

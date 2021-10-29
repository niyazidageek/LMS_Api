using System;
using System.Collections.Generic;

namespace Entities.DTOs
{
    public class QuestionDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Point { get; set; }

        public List<OptionDTO> Options { get; set; }

        public string FileName { get; set; }
    }
}

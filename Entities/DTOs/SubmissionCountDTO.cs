using System;
using System.Collections.Generic;

namespace Entities.DTOs
{
    public class SubmissionCountDTO
    {
        public List<int> Data { get; set; }

        public List<int> Years { get; set; }

        public int CurrentYear { get; set; }
    }
}

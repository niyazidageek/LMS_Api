using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Entities.DTOs
{
    public class QuestionAttachmentDTO
    {
        public IFormFile QuestionFile { get; set; }

        public string Values { get; set; }
    }
}

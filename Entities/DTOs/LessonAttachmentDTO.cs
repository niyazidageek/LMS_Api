using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Entities.DTOs
{
    public class LessonAttachmentDTO
    {
        public List<IFormFile> Materials { get; set; }

        public string Values { get; set; }
    }
}

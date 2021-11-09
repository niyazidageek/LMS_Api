using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Entities.DTOs
{
    public class SubmissionAttachmentDTO
    {
        public List<IFormFile> Files { get; set; }
    }
}

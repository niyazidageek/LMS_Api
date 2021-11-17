using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Entities.DTOs
{
    public class AssignmentAttachmentDTO
    {
        public List<IFormFile> Materials { get; set; }

        public string Values { get; set; }

        public bool NotifyAll { get; set; }
    }
}

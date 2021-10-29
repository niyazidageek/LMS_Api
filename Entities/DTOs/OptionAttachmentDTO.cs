using System;
using Microsoft.AspNetCore.Http;

namespace Entities.DTOs
{
    public class OptionAttachmentDTO
    {
        public IFormFile OptionFile { get; set; }

        public string Values { get; set; }
    }
}

using System;
using Microsoft.AspNetCore.Http;

namespace Entities.DTOs
{
    public class ProfilePictureAttachmentDTO
    {
        public IFormFile Picture { get; set; }

        public string Value { get; set; }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Entities.DTOs
{
    public class LessonDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public GroupDTO Group { get; set; }

        public List<MaterialDTO> Materials { get; set; }
    }
}

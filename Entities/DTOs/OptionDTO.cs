using System;
namespace Entities.DTOs
{
    public class OptionDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsCorrect { get; set; }

        public MaterialDTO Material { get; set; }
    }
}

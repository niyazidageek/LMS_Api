using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entities.Models
{
    public class GroupMaxPoint:IEntity
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public decimal MaxPoint { get; set; } = 0;

        public Group Group { get; set; }
    }
}

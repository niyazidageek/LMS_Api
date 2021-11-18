using System;
using Core.Entities;

namespace Entities.Models
{
    public class GroupSubmission:IEntity
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public DateTime Date { get; set; }
    }
}

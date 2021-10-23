using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entities.Models
{
    public class Subject:IEntity
    {
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }

        //public List<Group> Groups { get; set; }
    }
}


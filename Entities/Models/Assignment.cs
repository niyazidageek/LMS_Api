﻿using System;
using System.Collections.Generic;
using Core.Entities;

namespace Entities.Models
{
    public class Assignment:IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        public DateTime CreationDate { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public decimal MaxGrade { get; set; } = 0;

        public List<AssignmentMaterial> AssignmentMaterials { get; set; }

        public List<AssignmentAppUser> AssignmentAppUsers { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IAssignmentDal:IRepository<Assignment>
    {
        Task<List<Assignment>> GetAllByLessonIdAsync(int lessonId);

        Task<List<Assignment>> GetAssignmentsByLessonIdAndUserIdAsync(int lessonId, string appUserId)
    }
}

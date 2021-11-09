using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IAssignmentService
    {
        Task<List<Assignment>> GetAssignmentsByLessonIdAsync(int lessonId);
    }
}

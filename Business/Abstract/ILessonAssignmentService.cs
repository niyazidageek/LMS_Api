using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface ILessonAssignmentService
    {
        Task<List<LessonAssignment>> GetLessonAssignmentsByLessonIdAsync(int lessonId);

        Task<bool> CreateLessonAssignmentsAsync(List<LessonAssignment> lessonAssignments);

        Task<bool> DeleteLessonAssignmentsAsync(List<LessonAssignment> lessonAssignments);
    }
}

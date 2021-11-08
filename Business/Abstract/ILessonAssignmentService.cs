using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface ILessonAssignmentService
    {
        Task<List<LessonAssignment>> GetLessonAssignmentsByLessonId(int lessonId);

        Task<bool> CreateLessonAssignments(List<LessonAssignment> lessonAssignments);

        Task<bool> DeleteLessonAssignments(List<LessonAssignment> lessonAssignments);
    }
}

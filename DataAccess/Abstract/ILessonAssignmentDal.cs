using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface ILessonAssignmentDal: IRepository<LessonAssignment>
    {
        Task<List<LessonAssignment>> GetAllByLessonIdAsync(int id);

        Task<bool> CreateLessonAssignmentsAsync(List<LessonAssignment> lessonAssignments);

        Task<bool> DeleteLessonAssignmentsAsync(List<LessonAssignment> lessonAssignments);
    }
}

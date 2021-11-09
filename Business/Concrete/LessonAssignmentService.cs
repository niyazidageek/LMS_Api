using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class LessonAssignmentService:ILessonAssignmentService
    {
        private readonly ILessonAssignmentDal _conext;

        public LessonAssignmentService(ILessonAssignmentDal context)
        {
            _conext = context;
        }

        public async Task<List<LessonAssignment>> GetLessonAssignmentsByLessonIdAsync(int lessonId)
        {
            return await _conext.GetAllByLessonIdAsync(lessonId);
        }

        public async Task<bool> CreateLessonAssignmentsAsync(List<LessonAssignment> lessonAssignments)
        {
            await _conext.CreateLessonAssignmentsAsync(lessonAssignments);

            return true;
        }

        
        public async Task<bool> DeleteLessonAssignmentsAsync(List<LessonAssignment> lessonAssignments)
        {
            await _conext.DeleteLessonAssignmentsAsync(lessonAssignments);

            return true;
        }
    }
}

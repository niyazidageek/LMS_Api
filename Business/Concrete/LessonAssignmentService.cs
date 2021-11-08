using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class LessonAssignmentService:ILessonAssignmentService
    {
        private readonly ILessonAssignmentService _conext;

        public LessonAssignmentService(ILessonAssignmentService context)
        {
            _conext = context;
        }

        public async Task<List<LessonAssignment>> GetLessonAssignmentsByLessonId(int lessonId)
        {
            return await _conext.GetLessonAssignmentsByLessonId(lessonId);
        }

        public async Task<bool> CreateLessonAssignments(List<LessonAssignment> lessonAssignments)
        {
            await _conext.CreateLessonAssignments(lessonAssignments);

            return true;
        }

        
        public async Task<bool> DeleteLessonAssignments(List<LessonAssignment> lessonAssignments)
        {
            await _conext.DeleteLessonAssignments(lessonAssignments);

            return true;
        }
    }
}

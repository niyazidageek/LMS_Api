using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class AssignmentService:IAssignmentService
    {
        private readonly IAssignmentDal _context;

        public AssignmentService(IAssignmentDal context)
        {
            _context = context;
        }

        public async Task<List<Assignment>> GetAssignmentsByLessonIdAsync(int lessonId)
        {
            return await _context.GetAllByLessonIdAsync(lessonId);
        }
    }
}

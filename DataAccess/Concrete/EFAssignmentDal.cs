using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class EFAssignmentDal: EFRepositoryBase<Assignment, AppDbContext>, IAssignmentDal
    {
        public EFAssignmentDal(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Assignment>> GetAllByLessonIdAsync(int lessonId)
        {
            return await Context.Assignments
                .Where(a => a.LessonId == lessonId)
                .Include(a => a.AssignmentMaterials)
                .ToListAsync();
        }
    }
}

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
    public class EFTheoryDal:EFRepositoryBase<Theory, AppDbContext>, ITheoryDal
    {
        public EFTheoryDal(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Theory>> GetAllByLessonIdAsync(int lessonId)
        {
            return await Context.Theories.AsNoTracking()
                .Where(t => t.LessonId == lessonId)
                .ToListAsync();
        }
    }
}

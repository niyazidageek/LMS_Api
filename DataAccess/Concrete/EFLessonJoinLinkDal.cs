using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class EFLessonJoinLinkDal : EFRepositoryBase<LessonJoinLink, AppDbContext>, ILessonJoinLinkDal
    {
        public EFLessonJoinLinkDal(AppDbContext context) : base(context)
        {
        }

        public async Task<LessonJoinLink> GetByLessonIdAsync(int lessonId)
        {
            return await Context.LessonJoinLinks.AsNoTracking()
                .FirstOrDefaultAsync(lj => lj.LessonId == lessonId);
        }

        public async Task<bool> HasLessonStartedByLessonIdAsync(int lessonId)
        {
            return await Context.LessonJoinLinks.AsNoTracking()
                .AnyAsync(lj => lj.LessonId == lessonId);
        }
    }
}

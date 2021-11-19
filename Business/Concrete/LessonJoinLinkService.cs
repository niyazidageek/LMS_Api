using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class LessonJoinLinkService:ILessonJoinLinkService
    {
        private readonly ILessonJoinLinkDal _context;

        public LessonJoinLinkService(ILessonJoinLinkDal context)
        {
            _context = context;
        }

        public async Task<bool> AddLessonJoinLinkAsync(LessonJoinLink lessonJoinLink)
        {
            await _context.AddAsync(lessonJoinLink);

            return true;
        }

        public async Task<bool> DeleteLessonJoinLinkAsync(int id)
        {
            await _context.DeleteAsync(new LessonJoinLink { Id = id });

            return true;
        }

        public async Task<bool> EditLessonJoinLinkAsync(LessonJoinLink lessonJoinLink)
        {
            await _context.UpdateAsync(lessonJoinLink);

            return true;
        }

        public async Task<LessonJoinLink> GetLessonJoinLinkByIdAsync(int id)
        {
            return await _context.GetAsync(lj => lj.Id == id);
        }

        public async Task<LessonJoinLink> GetLessonJoinLinkByLessonIdAsync(int lessonId)
        {
            return await _context.GetByLessonIdAsync(lessonId);
        }

        public async Task<List<LessonJoinLink>> GetLessonJoinLinksAsync()
        {
            return await _context.GetAllAsync();
        }
    }
}

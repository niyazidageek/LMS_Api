using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class TheoryAppUserService:ITheoryAppUserService
    {
        private readonly ITheoryAppUserDal _context;

        public TheoryAppUserService(ITheoryAppUserDal context)
        {
            _context = context;
        }

        public async Task<bool> EditTheoryAppUserAsync(TheoryAppUser theoryAppUser)
        {
            await _context.UpdateAsync(theoryAppUser);

            return true;
        }

        public async Task<TheoryAppUser> GetTheoryAppUserByIdAsync(int id)
        {
            return await _context.GetAsync(ta => ta.Id == id);
        }

        public async Task<TheoryAppUser> GetTheoryAppUserByTheoryIdAndUserIdAsync(int theoryId, string userId)
        {
            return await _context.GetTheoryAppUserByTheoryIdAndUserIdAsync(theoryId, userId);
        }

        public async Task<List<TheoryAppUser>> GetTheoryAppUsersByAppUserIdAndGroupId(string userId, int groupId)
        {
            return await _context.GetTheoryAppUsersByAppUserIdAndGroupIdAsync(userId, groupId);
        }

        public async Task<List<TheoryAppUser>> GetTheoryAppUsersByLessonIdAsync(int lessonId)
        {
            return await _context.GetTheoryAppUsersByLessonIdAsync(lessonId);
        }

        public async Task<bool> InitializeTheoryAsync(Lesson lesson, int theoryId)
        {
            await _context.InitializeTheoryAsync(lesson, theoryId);

            return true;
        }
    }
}

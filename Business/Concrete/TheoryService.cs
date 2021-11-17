using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class TheoryService:ITheoryService
    {
        private readonly ITheoryDal _context;

        public TheoryService(ITheoryDal context)
        {
            _context = context;
        }

        public async Task<bool> AddTheoryAsync(Theory theory)
        {
            await _context.AddAsync(theory);

            return true;
        }

        public async Task<bool> DeleteTheoryAsync(int id)
        {
            await _context.DeleteAsync(new Theory { Id = id });

            return true;
        }

        public async Task<bool> EditTheoryAsync(Theory theory)
        {
            await _context.UpdateAsync(theory);

            return true;
        }

        public async Task<Theory> GetTheoryByIdAndUserId(int theoryId, string userId)
        {
            return await _context.GetByIdAndUserId(theoryId, userId);
        }

        public async Task<List<Theory>> GetTheoriesByLessonIdAndUserIdAsync(int lessonId, string appUserId)
        {
            return await _context.GetTheoriesByLessonIdAndUserIdAsync(lessonId, appUserId);
        }

        public async Task<List<Theory>> GetTheoriesByLessonIdAsync(int lessonId)
        {
            return await _context.GetAllByLessonIdAsync(lessonId);
        }

        public async Task<Theory> GetTheoryByIdAsync(int id)
        {
            return await _context.GetAsync(t => t.Id == id);
        }

        public async Task<List<Theory>> GetAllByGroupIdAsync(int groupId)
        {
            return await _context.GetAllByGroupIdAsync(groupId);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class LessonMaterialService:ILessonMaterialService
    {
        private readonly ILessonMaterialDal _context;

        public LessonMaterialService(ILessonMaterialDal context)
        {
            _context = context;
        }

        public async Task<bool> CreateLessonMaterialsAsync(List<LessonMaterial> lessonMaterials)
        {
            return await _context.AddAsync(lessonMaterials);
        }

        public async Task<bool> DeleteLessonMaterialsAsync(List<LessonMaterial> lessonMaterials)
        {
            return await _context.DeleteAsync(lessonMaterials);
        }

        public async Task<List<LessonMaterial>> GetLessonMaterialsByLessonIdAsync(int lessonId)
        {
            return await _context.GetAllByLessonIdAsync(lessonId);
        }
    }
}

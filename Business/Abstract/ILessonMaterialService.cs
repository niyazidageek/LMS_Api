using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface ILessonMaterialService
    {
        Task<List<LessonMaterial>> GetLessonMaterialsByLessonIdAsync(int lessonId);

        Task<bool> CreateLessonMaterialsAsync(List<LessonMaterial> lessonMaterials);

        Task<bool> DeleteLessonMaterialsAsync(List<LessonMaterial> lessonMaterials);
    }
}

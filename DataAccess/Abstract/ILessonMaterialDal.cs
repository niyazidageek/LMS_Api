using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface ILessonMaterialDal: IRepository<LessonMaterial>
    {
        Task<List<LessonMaterial>> GetAllByLessonIdAsync(int lessonId);

        Task<bool> AddAsync(List<LessonMaterial> lessonMaterials);

        Task<bool> DeleteAsync(List<LessonMaterial> lessonMaterials);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface ILessonDal:IRepository<Lesson>
    {
        Task<bool> AddWithFilesAsync(Lesson lesson, List<string> fileNames);
    }
}

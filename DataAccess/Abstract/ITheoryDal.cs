using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface ITheoryDal: IRepository<Theory>
    {
        Task<List<Theory>> GetAllByLessonIdAsync(int lessonId);
    }
}

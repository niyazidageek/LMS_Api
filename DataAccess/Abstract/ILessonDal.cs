using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;

namespace DataAccess.Abstract
{
    public interface ILessonDal:IRepository<Lesson>
    {
        Task<bool> AddWithFilesAsync(Lesson lesson);

        Task<bool> EditWithFilesAsync(Lesson lesson);

        Task<bool> EditAsync(Lesson lesson);

        Task<bool> DeleteWithFilesAsync(Lesson lesson);
    }
}

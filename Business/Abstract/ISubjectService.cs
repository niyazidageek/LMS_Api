using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface ISubjectService
    {
        Task<List<Subject>> GetSubjectsAsync();

        Task<Subject> GetSubjectByIdAsync(int id);

        Task<bool> AddSubjectAsync(Subject subject);

        Task<bool> EditSubjectAsync(Subject subject);

        Task<bool> DeleteSubjectAsync(int id);
    }
}

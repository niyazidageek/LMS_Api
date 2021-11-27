using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface ISubjectDal:IRepository<Subject>
    {
        Task<List<Subject>> GetByPageAndSizeAsync(int page, int size);

        Task<int> GetSubjectsCountAsync();
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IApplicationDal : IRepository<Application>
    {
        Task<Application> GetAsync(int id);

        Task<int> GetApplicationsCountAsync();

        Task<List<Application>> GetByPageAndSizeAsync(int page, int size);
    }
}

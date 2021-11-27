using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IApplicationService
    {
        Task<Application> GetApplicationByIdAsync(int id);

        Task<bool> AddApplicationAsync(Application application);

        Task<bool> DeleteApplicationAsync(int id);

        Task<List<Application>> GetApplicationsByPageAndSizeAsync(int page, int size);

        Task<int> GetApplicationsCountAsync();
    }
}

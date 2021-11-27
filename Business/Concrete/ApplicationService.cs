using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class ApplicationService:IApplicationService
    {
        private readonly IApplicationDal _context;

        public ApplicationService(IApplicationDal context)
        {
            _context = context;
        }

        public async Task<bool> AddApplicationAsync(Application application)
        {
            await _context.AddAsync(application);

            return true;
        }

        public async Task<bool> DeleteApplicationAsync(int id)
        {
            await _context.DeleteAsync(new Application { Id = id });

            return true;
        }

        public async Task<Application> GetApplicationByIdAsync(int id)
        {
            return await _context.GetAsync(id);
        }

        public async Task<List<Application>> GetApplicationsByPageAndSizeAsync(int page, int size)
        {
            return await _context.GetByPageAndSizeAsync(page, size);
        }

        public async Task<int> GetApplicationsCountAsync()
        {
            return await _context.GetApplicationsCountAsync();
        }
    }
}

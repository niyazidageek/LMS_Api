using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class EFApplicationDal : EFRepositoryBase<Application, AppDbContext>, IApplicationDal
    {
        public EFApplicationDal(AppDbContext context):base(context)
        {
        }

        public async Task<Application> GetAsync(int id)
        {
            return await Context.Applications.AsNoTracking()
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<int> GetApplicationsCountAsync()
        {
            return await Context.Applications.AsNoTracking()
                 .CountAsync();
        }

        public async Task<List<Application>> GetByPageAndSizeAsync(int page, int size)
        {
            return await Context.Applications.AsNoTracking()
                .OrderByDescending(a => a.Id)
                .Skip(page * size)
                .Take(size)
                .ToListAsync();
        }
    }
}

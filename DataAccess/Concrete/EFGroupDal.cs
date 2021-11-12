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
    public class EFGroupDal: EFRepositoryBase<Group, AppDbContext>, IGroupDal
    {

        public EFGroupDal(AppDbContext context) : base(context)
        {
        }

        public async Task<Group> GetAsync(int id)
        {
            return await Context.Groups.AsNoTracking()
                .Include(g => g.Subject)
                .Include(g=>g.AppUserGroups)
                .ThenInclude(aug=>aug.AppUser)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<List<Group>> GetAllAsync()
        {
            return await Context.Groups.AsNoTracking()
                .Include(g => g.Subject)
                .Include(g => g.AppUserGroups)
                .ToListAsync();
        }

        public async Task<List<Group>> GetByCountAsync(int skipCount, int takeCount)
        {
            return await Context.Groups.Include(g => g.Subject)
                .Include(g => g.AppUserGroups)
                .ThenInclude(g=>g.AppUser)
                .Skip(skipCount).Take(takeCount)
                .ToListAsync();
        }

        public async Task<List<Group>> GetGroupsByUserIdAsync(string userId)
        {
            var groups = await Context.Groups.AsNoTracking()
                .Include(g => g.AppUserGroups)
                .ThenInclude(g => g.AppUser)
                .Where(g => g.AppUserGroups.All(ag => ag.AppUserId == userId))
                .ToListAsync();

            return groups;
        }
    }
}

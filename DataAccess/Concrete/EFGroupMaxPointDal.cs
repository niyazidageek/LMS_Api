using System;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class EFGroupMaxPointDal: EFRepositoryBase<GroupMaxPoint, AppDbContext>, IGroupMaxPointDal
    {
        public EFGroupMaxPointDal(AppDbContext context) : base(context)
        {
        }

        public async Task<GroupMaxPoint> GetByGroupIdAsync(int groupId)
        {
            return await Context.GroupMaxPoints.AsNoTracking()
                .FirstOrDefaultAsync(gmp => gmp.GroupId == groupId);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using MoreLinq.Extensions;

namespace DataAccess.Concrete
{
    public class EFGroupSubmissionDal: EFRepositoryBase<GroupSubmission, AppDbContext>, IGroupSubmissionDal
    {
        public EFGroupSubmissionDal(AppDbContext context) : base(context)
        {
        }

        public async Task<List<GroupSubmission>> GetAllGroupSubmissionsByGroupIdAndYearAsync(int groupId, int year)
        {
            return await Context.GroupSubmissions
                .AsNoTracking()
                .Where(gs => gs.GroupId == groupId && gs.Date.Year == year)
                .ToListAsync();
        }

        public async Task<List<int>> GetPossibleYearsAsync(int groupId)
        {
            var groupSubmissionsDb = await Context.GroupSubmissions
                .AsNoTracking()
                .Where(gs => gs.GroupId == groupId)
                .ToListAsync();

            return groupSubmissionsDb.DistinctBy(gs => gs.Date.Year)
                .Select(gs=>gs.Date.Year).ToList();
        }
    }
}

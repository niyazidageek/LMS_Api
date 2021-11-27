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
    public class EFSubjectDal:EFRepositoryBase<Subject, AppDbContext>, ISubjectDal
    {
        public EFSubjectDal(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Subject>> GetByPageAndSizeAsync(int page, int size)
        {
            return await Context.Subjects.AsNoTracking()
                .OrderByDescending(s => s.Id)
                .Skip(page * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> GetSubjectsCountAsync()
        {
            return await Context.Subjects.AsNoTracking()
                 .CountAsync();
        }
    }
}

using System;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;

namespace DataAccess.Concrete
{
    public class EFGroupDal: EFRepositoryBase<Group, AppDbContext>, IGroupDal
    {
        public EFGroupDal(AppDbContext context) : base(context)
        {

        }
    }
}

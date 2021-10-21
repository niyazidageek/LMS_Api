using System;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;

namespace DataAccess.Concrete
{
    public class EFSubjectDal:EFRepositoryBase<Subject, AppDbContext>, ISubjectDal
    {
        public EFSubjectDal(AppDbContext context) : base(context)
        {

        }
    }
}

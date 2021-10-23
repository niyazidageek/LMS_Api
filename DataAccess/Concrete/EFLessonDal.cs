using System;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;

namespace DataAccess.Concrete
{
    public class EFLessonDal: EFRepositoryBase<Lesson, AppDbContext>, ILessonDal
    {
        public EFLessonDal(AppDbContext context) : base(context)
        {
        }
    }
}

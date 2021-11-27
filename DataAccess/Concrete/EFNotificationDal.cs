using System;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;

namespace DataAccess.Concrete
{
    public class EFNotificationDal : EFRepositoryBase<Notification, AppDbContext>, INotificationDal
    {
        public EFNotificationDal(AppDbContext context) : base(context)
        {
        }
    }
}

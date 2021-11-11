using System;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IGroupMaxPointDal : IRepository<GroupMaxPoint>
    {
        Task<GroupMaxPoint> GetByGroupIdAsync(int groupId);
    }
}

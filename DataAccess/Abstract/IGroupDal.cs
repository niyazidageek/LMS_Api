using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IGroupDal:IRepository<Group>
    {
        Task<List<Group>> GetByCountAsync(int skipCount, int takeCount);

        Task<Group> GetAsync(int id);

        Task<bool> RelationalUpdateAsync(Group group);
    }
}

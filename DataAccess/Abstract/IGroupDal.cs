using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IGroupDal:IRepository<Group>
    {
        Task<List<Group>> GetByCountAsync(int page, int size);

        Task<Group> GetAsync(int id);

        Task<List<Group>> GetAllAsync();

        Task<List<Group>> GetGroupsByUserIdAsync(string userId);

        Task<int> GetGroupsCountAsync();
    }
}

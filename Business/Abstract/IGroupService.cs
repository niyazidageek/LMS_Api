using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IGroupService
    {
        Task<List<Group>> GetGroupsAsync();

        Task<List<Group>> GetGroupsByCountAsync(int page, int size);

        Task<Group> GetGroupDetailsByIdAsync(int id);

        Task<Group> GetGroupByIdAsync(int id);

        Task<bool> AddGroupAsync(Group group);

        Task<bool> EditGroupAsync(Group group);

        Task<bool> DeleteGroupAsync(int id);

        Task<List<Group>> GetGroupsByUserIdAsync(string userId);

        Task<int> GetGroupsCountAsync();
    }
}

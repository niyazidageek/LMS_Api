using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class GroupService : IGroupService
    {
        private readonly IGroupDal _context;

        public GroupService(IGroupDal context)
        {
            _context = context;
        }

        public async Task<bool> AddGroupAsync(Group group)
        {
            await _context.AddAsync(group);

            return true;
        }

        public async Task<bool> DeleteGroupAsync(int id)
        {
            await _context.DeleteAsync(new Group { Id = id });

            return true;
        }

        public async Task<bool> EditGroupAsync(Group group)
        {
            await _context.UpdateAsync(group);

            return true;
        }

        public async Task<Group> GetGroupByIdAsync(int id)
        {
            return await _context.GetAsync(g => g.Id == id);
        }

        public async Task<List<Group>> GetGroupsAsync()
        {
            return await _context.GetAllAsync();
        }
    }
}

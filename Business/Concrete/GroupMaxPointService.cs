using System;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class GroupMaxPointService:IGroupMaxPointService
    {
        private readonly IGroupMaxPointDal _context;

        public GroupMaxPointService(IGroupMaxPointDal context)
        {
            _context = context;
        }

        public async Task<bool> EditGroupMaxPoint(GroupMaxPoint groupMaxPoint)
        {
            await _context.UpdateAsync(groupMaxPoint);

            return true;
        }

        public async Task<GroupMaxPoint> GetGroupMaxPointByGroupId(int groupId)
        {
            return await _context.GetByGroupIdAsync(groupId);
        }
    }
}

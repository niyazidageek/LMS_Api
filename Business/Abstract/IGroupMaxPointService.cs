using System;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IGroupMaxPointService
    {
        Task<GroupMaxPoint> GetGroupMaxPointByGroupId(int groupId);

        Task<bool> EditGroupMaxPoint(GroupMaxPoint groupMaxPoint);
    }
}

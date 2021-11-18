using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class GroupSubmissionService:IGroupSubmissionService
    {
        private readonly IGroupSubmissionDal _context;

        public GroupSubmissionService(IGroupSubmissionDal context)
        {
            _context = context;
        }

        public async Task<bool> AddGroupSubmissionAsync(GroupSubmission groupSubmission)
        {
            await _context.AddAsync(groupSubmission);

            return true;
        }

        public async Task<bool> DeleteGroupSubmissionAsync(int id)
        {
            await _context.DeleteAsync(new GroupSubmission { Id = id });

            return true;
        }

        public async Task<bool> EditGroupSubmissionAsync(GroupSubmission groupSubmission)
        {
            await _context.UpdateAsync(groupSubmission);

            return true;
        }

        public async Task<List<GroupSubmission>> GetAllGroupSubmissionsByGroupIdAndYear(int groupId, int year)
        {
            return await _context.GetAllGroupSubmissionsByGroupIdAndYear(groupId, year);
        }

        public async Task<GroupSubmission> GetGroupSubmissionByIdAsync(int id)
        {
            return await _context.GetAsync(gs => gs.Id == id);
        }

        public async Task<List<GroupSubmission>> GetGroupSubmissionsAsync()
        {
            return await _context.GetAllAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IGroupSubmissionDal: IRepository<GroupSubmission>
    {
        Task<List<GroupSubmission>> GetAllGroupSubmissionsByGroupIdAndYearAsync(int groupId, int year);

        Task<List<int>> GetPossibleYearsAsync(int groupId);
    }
}

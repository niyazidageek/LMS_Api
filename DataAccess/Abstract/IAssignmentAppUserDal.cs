using System;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IAssignmentAppUserDal: IRepository<AssignmentAppUser>
    {
        Task<bool> InitializeAssignmentAsync(Lesson lesson, int assignmentId);
    }
}

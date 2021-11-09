using System;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IAssignmentAppUserService
    {
        //Task<List<Subject>> GetSubjectsAsync();

        //Task<Subject> GetSubjectByIdAsync(int id);

        Task<bool> InitializeAssignmentAsync(Lesson lesson, int assignmentId);

        //Task<bool> SubmitAssginment(Subject subject);

        //Task<bool> EditSubjectAsync(Subject subject);

        //Task<bool> DeleteSubjectAsync(int id);
    }
}

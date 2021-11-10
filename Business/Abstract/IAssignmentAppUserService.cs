using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IAssignmentAppUserService
    {
        //Task<List<Subject>> GetSubjectsAsync();

        Task<List<AssignmentAppUser>> GetAssignmentAppUsersByLessonIdAsync(int lessonId);

        Task<AssignmentAppUser> GetAssignmentAppUserByIdAsync(int id);

        Task<bool> InitializeAssignmentAsync(Lesson lesson, int assignmentId);

        //Task<bool> SubmitAssginment(Subject subject);

        Task<bool> EditAssignmentAppUserAsync(AssignmentAppUser assignmentAppUser);

        Task<AssignmentAppUser> GetAssignmentAppUserByAssignmentIdAndUserIdAsync(int assignmentId, string userId);

        //Task<bool> DeleteSubjectAsync(int id);
    }
}

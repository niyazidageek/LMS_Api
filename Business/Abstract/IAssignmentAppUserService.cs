using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IAssignmentAppUserService
    {
        //Task<List<Subject>> GetSubjectsAsync();

        Task<List<AssignmentAppUser>> GetAssignmentAppUsersByLessonIdAndUserIdAsync(int lessonId, string appUserId);

        Task<AssignmentAppUser> GetAssignmentAppUserByIdAsync(int id);

        Task<bool> InitializeAssignmentAsync(Lesson lesson, int assignmentId);

        //Task<bool> SubmitAssginment(Subject subject);

        Task<bool> EditAssignmentAppUserAsync(AssignmentAppUser assignmentAppUser);

        //Task<bool> DeleteSubjectAsync(int id);
    }
}

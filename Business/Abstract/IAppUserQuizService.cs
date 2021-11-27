using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IAppUserQuizService
    {
        Task<bool> InitializeQuizAsync(List<AppUserGroup> appUserGroups, int quizId);

        Task<AppUserQuiz> GetAppUserQuizByUserIdAndQuizIdAsync(string userId, int quizId);

        Task<bool> EditAppUserQuizAsync(AppUserQuiz appUserQuiz);
    }
}

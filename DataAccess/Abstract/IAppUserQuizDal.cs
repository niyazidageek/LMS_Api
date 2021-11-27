using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IAppUserQuizDal : IRepository<AppUserQuiz>
    {
        Task<bool> InitializeQuizAsync(List<AppUserGroup> appUserGroups, int quizId);

        Task<AppUserQuiz> GetByUserIdAndQuizIdAsync(string userId, int quizId);
    }
}

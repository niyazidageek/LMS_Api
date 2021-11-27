using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class AppUserQuizService:IAppUserQuizService
    {
        private readonly IAppUserQuizDal _context;

        public AppUserQuizService(IAppUserQuizDal context)
        {
            _context = context;
        }

        public async Task<bool> EditAppUserQuizAsync(AppUserQuiz appUserQuiz)
        {
            await _context.UpdateAsync(appUserQuiz);

            return true;
        }

        public async Task<AppUserQuiz> GetAppUserQuizByUserIdAndQuizIdAsync(string userId, int quizId)
        {
            return await _context.GetByUserIdAndQuizIdAsync(userId, quizId);
        }

        public async Task<bool> InitializeQuizAsync(List<AppUserGroup> appUserGroups, int quizId)
        {
            await _context.InitializeQuizAsync(appUserGroups, quizId);

            return true;
        }
    }
}

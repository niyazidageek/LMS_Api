using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IAppUserOptionService
    {
        Task<List<AppUserOption>> GetAppUserOptionsAsync();

        Task<AppUserOption> GetAppUserOptionByIdAsync(int id);

        Task<bool> AddAppUserOptionAsync(AppUserOption appUserOption);

        Task<bool> EditAppUserOptionAsync(AppUserOption appUserOption);

        Task<bool> DeleteAppUserOptionAsync(int id);

        Task<List<AppUserOption>> GetAppUserOptionsByQuizIdAndUserIdAsync(int quizId, string userId);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class AppUserOptionService:IAppUserOptionService
    {
        private readonly IAppUserOptionDal _context;

        public AppUserOptionService(IAppUserOptionDal context)
        {
            _context = context;
        }

        public async Task<bool> AddAppUserOptionAsync(AppUserOption appUserOption)
        {
            await _context.AddAsync(appUserOption);

            return true;
        }

        public async Task<bool> DeleteAppUserOptionAsync(int id)
        {
            await _context.DeleteAsync(new AppUserOption { Id = id });

            return true;
        }

        public async Task<bool> EditAppUserOptionAsync(AppUserOption appUserOption)
        {
            await _context.UpdateAsync(appUserOption);

            return true;
        }

        public async Task<AppUserOption> GetAppUserOptionByIdAsync(int id)
        {
            return await _context.GetAsync(ao => ao.Id == id);
        }

        public async Task<List<AppUserOption>> GetAppUserOptionsAsync()
        {
            return await _context.GetAllAsync();
        }

        public async Task<List<AppUserOption>> GetAppUserOptionsByQuizIdAndUserIdAsync(int quizId, string userId)
        {
            return await _context.GetAllByQuizIdAndUserIdAsync(quizId, userId);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class OptionService:IOptionService
    {
        private readonly IOptionDal _context;

        public OptionService(IOptionDal context)
        {
            _context = context;
        }

        public async Task<bool> AddOptionAsync(Option option)
        {
            await _context.AddAsync(option);

            return true;
        }

        public async Task<bool> AddOptionWithFileAsync(Option option)
        {
            await _context.AddWithFileAsync(option);

            return true;
        }

        public async Task<bool> DeleteOptionAsync(Option option)
        {
            await _context.DeleteAsync(option);

            return true;
        }

        public async Task<bool> DeleteQuestionWithFileAsync(Option option)
        {
            await _context.DeleteWithFileAsync(option);

            return true;
        }

        public async Task<bool> EditOptionAsync(Option option)
        {
            await _context.UpdateAsync(option);

            return true;
        }

        public async Task<bool> EditQuestionWithFileAsync(Option option)
        {
            await _context.UpdateWithFileAsync(option);

            return true;
        }

        public async Task<bool> EditQuestionWithoutFileAsync(Option option)
        {
            await _context.UpdateWithoutFileAsync(option);

            return true;
        }

        public async Task<Option> GetOptionByIdAsync(int id)
        {
            return await _context.GetAsync(id);
        }

        public async Task<List<Option>> GetOptionsAsync()
        {
            return await _context.GetAllAsync();
        }

        public async Task<List<Option>> GetOptionsByQuestionAsync(int id)
        {
            return await _context.GetOptionsByQuestion(id);
        }
    }
}

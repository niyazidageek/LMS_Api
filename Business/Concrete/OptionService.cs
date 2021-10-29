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

        public Task<bool> DeleteOptionAsync(Option option)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditOptionAsync(Option option)
        {
            throw new NotImplementedException();
        }

        public Task<Option> GetOptionByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Option>> GetOptionsAsync()
        {
            throw new NotImplementedException();
        }
    }
}

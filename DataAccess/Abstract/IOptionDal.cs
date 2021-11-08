using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IOptionDal: IRepository<Option>
    {
        Task<List<Option>> GetOptionsByQuestion(int id);

        Task<bool> AddWithFileAsync(Option option);

        Task<List<Option>> GetAllAsync();

        Task<bool> UpdateWithFileAsync(Option option);

        Task<bool> UpdateWithoutFileAsync(Option option);

        Task<bool> DeleteWithFileAsync(Option option);

        Task<Option> GetAsync(int id);
    }
}

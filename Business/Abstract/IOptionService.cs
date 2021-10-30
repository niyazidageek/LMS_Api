using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Business.Abstract
{
    public interface IOptionService
    {
        Task<List<Option>> GetOptionsAsync();

        Task<List<Option>> GetOptionsByQuestionAsync(int id);

        Task<Option> GetOptionByIdAsync(int id);

        Task<bool> AddOptionAsync(Option option);

        Task<bool> AddOptionWithFileAsync(Option option);

        Task<bool> EditOptionAsync(Option option);

        Task<bool> DeleteOptionAsync(Option option);

        Task<bool> EditQuestionWithFileAsync(Option option);

        Task<bool> EditQuestionWithoutFileAsync(Option option);

        Task<bool> DeleteQuestionWithFileAsync(Option option);
    }
}

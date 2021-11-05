using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IQuizDal: IRepository<Quiz>
    {
        Task<bool> DeleteWithQuestionsAndOptionsAsync(int id);

        Task<List<Quiz>> GetAllAsync();

        Task<Quiz> GetAsync(int id);
    }
}

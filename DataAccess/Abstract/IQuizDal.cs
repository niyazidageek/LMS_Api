using System;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IQuizDal: IRepository<Quiz>
    {
        Task<bool> DeleteWithQuestionsAndOptionsAsync(int id);
    }
}

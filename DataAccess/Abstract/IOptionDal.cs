using System;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IOptionDal: IRepository<Option>
    {
        Task<bool> AddWithFileAsync(Option option);
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Repository
{
    public interface IRepository<T> where T: class, IEntity, new()
    {
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null);

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);

        Task<bool> AddAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> DeleteAsync(T entity);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository.EFRepository
{
    public class EFRepositoryBase<TEntity, IContext> : IRepository<TEntity> where TEntity : class, IEntity, new()
                                                                           where IContext : DbContext, new()
    {
        protected readonly IContext Context;

        public EFRepositoryBase(IContext context)
        {
            Context = context;
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                await Context.Set<TEntity>().AddAsync(entity);
                await Context.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> DeleteAsync(TEntity entity)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                Context.Set<TEntity>().Remove(entity);
                await Context.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter == null)
            {
                return await Context.Set<TEntity>().AsNoTracking().ToListAsync();
            }

            return await Context.Set<TEntity>().AsNoTracking().Where(filter).ToListAsync();
        }


        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null ? await Context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync()
                                  : await Context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filter);
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                Context.Set<TEntity>().Update(entity);
                await Context.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync();
                throw;
            }
        }
    }
}

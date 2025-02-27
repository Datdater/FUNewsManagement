using BusinessLogicLayer.Contracts;
using DataAccessLayer.DatabaseConfiguration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly NewsManagementDB _context;
        protected readonly DbSet<T> _dbSet;
        protected IQueryable<T> _query;

        public GenericRepository(NewsManagementDB context)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _query = _dbSet;
        }

        public virtual IGenericRepository<T> Include(params Expression<Func<T, object>>[] includes)
        {
            foreach (var include in includes)
            {
                _query = _query.Include(include);
            }
            return this;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            var entityType = _context.Model.FindEntityType(typeof(T));
            var primaryKey = entityType?.FindPrimaryKey()?.Properties.FirstOrDefault()?.Name;
            // First try to find the entity using the query with includes
            var result = await _query.FirstOrDefaultAsync(e => EF.Property<int>(e, primaryKey) == id);

            // Reset query for next operation
            _query = _dbSet;

            return result;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = await _query.ToListAsync();

            // Reset query for next operation
            _query = _dbSet;

            return result;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
    }
}
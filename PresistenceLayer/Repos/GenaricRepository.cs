using DomainLayer.Contract;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using PresistenceLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PresistenceLayer.Repos
{
    public class GenaricRepository<TEntity, TKey>(EventDbContext _eventDbContext)
                        : IGenaricRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>, new()
    {
        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> criteria, string[] includes = null)
        {
            var entity = _eventDbContext.Set<TEntity>().AsTracking();

            if (includes != null)
                foreach (var include in includes ?? Array.Empty<string>())
                    entity = entity.Include(include);

            return await entity.FirstOrDefaultAsync(criteria);
        }
        public async Task<int> AddAsync(TEntity entity)
        {
             await _eventDbContext.Set<TEntity>().AddAsync(entity);
             return await _eventDbContext.SaveChangesAsync();
        }

        public async Task<bool> Delete(TEntity entity)
        {
                _eventDbContext.Set<TEntity>().Remove(entity);
                return await _eventDbContext.SaveChangesAsync() > 0;
        }



        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _eventDbContext.Set<TEntity>().ToListAsync();



        public async Task<TEntity?> GetByIdAsync(TKey id)
            => await _eventDbContext.Set<TEntity>().FindAsync(id);


        public async Task<bool> Update(TEntity entity)
        { 
            _eventDbContext.Set<TEntity>().Update(entity);
            return await _eventDbContext.SaveChangesAsync() > 0 ;

        }
    }
}

using DomainLayer.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contract
{
    public interface IGenaricRepository<TEntity,TKey>  where TEntity : BaseEntity<TKey> , new()
    {
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> criteria, string[] includes = null);
        IQueryable<TDto> FindAllAsync<TDto>(Expression<Func<TEntity, bool>> criteria, string[] includes = null); 
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<int> AddAsync(TEntity entity);
        Task<bool> Update(TEntity entity);
        Task<bool> Delete(TEntity entity);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    }
}

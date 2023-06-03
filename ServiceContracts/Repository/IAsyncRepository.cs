using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Repository
{
    public interface IAsyncRepository<T> where T : class
    {
        IQueryable<T> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    List<Expression<Func<T, object>>> includes = null, bool disableTracking = true);
        Task<T> GetByIdAsync(Guid id);
        Task<T> Find(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate=null);
        T Add(T entity);
        Task AddRangeAsync(List<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRangeAsync(List<T> entities);
    }
}

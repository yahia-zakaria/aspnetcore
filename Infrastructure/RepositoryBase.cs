using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts.Repository;
using System.Linq.Expressions;

namespace Infrastructure
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : class
    {
        private readonly ApplicationDbContext context;
        public RepositoryBase(ApplicationDbContext context)
        {
            this.context = context;
        }
        public T Add(T entity)
        {
            context.Set<T>().Add(entity);
            return entity;
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            await context.Set<T>().AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate is not null)
                return (await CountAsync(predicate)) > 0;

            return await context.Set<T>().CountAsync() > 0;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate is not null)
                return await context.Set<T>().CountAsync(predicate);

            return await context.Set<T>().CountAsync();
        }

        public async Task<T>? Find(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public IQueryable<T> GetAllAsync()
        {
            return context.Set<T>();
        }
       

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, List<Expression<Func<T, object>>>? includes = null, bool disableTracking = true)
        {
            IQueryable<T> query = context.Set<T>();

            if (disableTracking)
                query = query.AsNoTracking();

            if (!(includes is null))
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (!(predicate is null))
                query = query.Where(predicate);

            if (!(orderBy is null))
                return await orderBy(query).ToListAsync();

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public void Remove(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public void RemoveRangeAsync(List<T> entities)
        {
            context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            context.Entry<T>(entity).State = EntityState.Modified;
        }
    }
}
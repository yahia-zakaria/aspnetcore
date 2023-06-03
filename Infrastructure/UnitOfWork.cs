using Entities;
using ServiceContracts.Repository;
using System.Transactions;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        private readonly Dictionary<Type, object> _repositories;
        private TransactionScope transaction;
        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public void CreateTransaction()
        {
            transaction = new TransactionScope();
        }
        public void CommitTransaction()
        {
            transaction.Complete();
        }
        public IAsyncRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IAsyncRepository<T>)_repositories[typeof(T)];
            }

            var repository = new RepositoryBase<T>(context);
            _repositories.Add(typeof(T), repository);

            return repository;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
                if (transaction != null)
                    transaction.Dispose();
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
